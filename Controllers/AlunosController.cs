using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoFinalPortfolio.Data;
using ProjetoFinalPortfolio.models;
using ProjetoFinalPortfolio.models.DTOs;

namespace ProjetoFinalPortfolio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AlunosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CriarAlunoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Nome e e-mail são obrigatórios" });

            if (!await _context.Turmas.AnyAsync(t => t.Id == dto.TurmaId))
                return BadRequest(new { message = "O aluno deve estar associado a uma turma existente" });

            if (await _context.Alunos.AnyAsync(a => a.Email == dto.Email))
                return Conflict(new { message = "Já existe um aluno com esse e-mail" });

            var aluno = new Aluno
            {
                Nome = dto.Nome,
                Email = dto.Email,
                TurmaId = dto.TurmaId
            };

            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Aluno criado com sucesso", id = aluno.Id });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlunoInfoDto>>> GetAlunos([FromQuery] string? nome, [FromQuery] int? turmaId)
        {
            var alunos = await _context.Alunos
                .AsNoTracking()
                .Where(a => string.IsNullOrEmpty(nome) || a.Nome.Contains(nome))
                .Where(a => !turmaId.HasValue || a.TurmaId == turmaId.Value)
                .OrderBy(a => a.Nome)
                .Select(a => new AlunoInfoDto
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Email = a.Email,
                    TurmaId = a.TurmaId,
                    TurmaNome = a.Turma!.Nome
                })
                .ToListAsync();

            return Ok(alunos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlunoInfoDto>> GetAlunoById(int id)
        {
            var aluno = await _context.Alunos
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new AlunoInfoDto
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Email = a.Email,
                    TurmaId = a.TurmaId,
                    TurmaNome = a.Turma!.Nome
                })
                .FirstOrDefaultAsync();

            if (aluno == null)
            {
                return NotFound(new { message = "Aluno não encontrado" });
            }

            return Ok(aluno);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] CriarAlunoDto dto)
        {
            var aluno = await _context.Alunos.FindAsync(id);

            if (aluno == null)
                return NotFound(new { message = "Aluno não encontrado" });

            if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Nome e e-mail são obrigatórios" });

            if (!await _context.Turmas.AnyAsync(t => t.Id == dto.TurmaId))
                return BadRequest(new { message = "O aluno deve estar associado a uma turma existente" });

            if (await _context.Alunos.AnyAsync(a => a.Email == dto.Email && a.Id != id))
                return Conflict(new { message = "Já existe um aluno com esse e-mail" });

            aluno.Nome = dto.Nome;
            aluno.Email = dto.Email;
            aluno.TurmaId = dto.TurmaId;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Aluno atualizado com sucesso" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);

            if (aluno == null)
                return NotFound(new { message = "Aluno não encontrado" });

            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Aluno excluído com sucesso" });
        }
    }
}