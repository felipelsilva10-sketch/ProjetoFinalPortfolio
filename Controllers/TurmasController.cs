using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoFinalPortfolio.Data;
using ProjetoFinalPortfolio.models;
using ProjetoFinalPortfolio.models.DTOs;

namespace ProjetoFinalPortfolio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurmasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TurmasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CriarTurmaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                return BadRequest(new { message = "O nome da turma é obrigatório" });

            var turma = new Turma
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao
            };

            _context.Turmas.Add(turma);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Turma criada com sucesso", id = turma.Id });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TurmaInfoDto>>> GetTurmas([FromQuery] string? nome)
        {
            var turmas = await _context.Turmas
                .AsNoTracking()
                .Where(t => string.IsNullOrEmpty(nome) || t.Nome.Contains(nome))
                .OrderBy(t => t.Nome)
                .Select(t => new TurmaInfoDto
                {
                    Id = t.Id,
                    Nome = t.Nome,
                    Descricao = t.Descricao
                })
                .ToListAsync();

            return Ok(turmas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TurmaInfoDto>> GetTurmaById(int id)
        {
            var turma = await _context.Turmas
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new TurmaInfoDto
                {
                    Id = t.Id,
                    Nome = t.Nome,
                    Descricao = t.Descricao
                })
                .FirstOrDefaultAsync();

            if (turma == null)
            {
                return NotFound(new { message = "Turma não encontrada" });
            }

            return Ok(turma);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] CriarTurmaDto dto)
        {
            var turma = await _context.Turmas.FindAsync(id);

            if (turma == null)
                return NotFound(new { message = "Turma não encontrada" });

            if (string.IsNullOrWhiteSpace(dto.Nome))
                return BadRequest(new { message = "O nome da turma é obrigatório" });

            turma.Nome = dto.Nome;
            turma.Descricao = dto.Descricao;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Turma atualizada com sucesso" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var turma = await _context.Turmas.FindAsync(id);

            if (turma == null)
                return NotFound(new { message = "Turma não encontrada" });

            var temAlunos = await _context.Alunos.AnyAsync(a => a.TurmaId == id);
            if (temAlunos)
                return Conflict(new { message = "Não é possível excluir uma turma que possui alunos" });

            _context.Turmas.Remove(turma);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Turma excluída com sucesso" });
        }
    }
}