using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoFinalPortfolio.Data;
using ProjetoFinalPortfolio.models;
using ProjetoFinalPortfolio.models.DTOs;

namespace ProjetoFinalPortfolio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjetosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CriarProjetoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Titulo) ||
                string.IsNullOrWhiteSpace(dto.Descricao) ||
                string.IsNullOrWhiteSpace(dto.Tecnologias))
                return BadRequest(new { message = "Título, descrição e tecnologias são obrigatórios" });

            if (!await _context.Alunos.AnyAsync(a => a.Id == dto.AlunoId))
                return BadRequest(new { message = "O projeto deve pertencer a um aluno existente" });

            var projeto = new Projeto
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Tecnologias = dto.Tecnologias,
                LinkAcesso = dto.LinkAcesso,
                ImagemUrl = dto.ImagemUrl,
                VideoUrl = dto.VideoUrl,
                AlunoId = dto.AlunoId
            };

            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Projeto criado com sucesso", id = projeto.Id });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjetoInfoDto>>> GetProjetos([FromQuery] string? titulo, [FromQuery] int? alunoId)
        {
            var projetos = await _context.Projetos
                .AsNoTracking()
                .Where(p => string.IsNullOrEmpty(titulo) || p.Titulo.Contains(titulo))
                .Where(p => !alunoId.HasValue || p.AlunoId == alunoId.Value)
                .OrderBy(p => p.Titulo)
                .Select(p => new ProjetoInfoDto
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    Descricao = p.Descricao,
                    Tecnologias = p.Tecnologias,
                    LinkAcesso = p.LinkAcesso,
                    ImagemUrl = p.ImagemUrl,
                    VideoUrl = p.VideoUrl,
                    AlunoId = p.AlunoId,
                    AlunoNome = p.Aluno!.Nome
                })
                .ToListAsync();

            return Ok(projetos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjetoInfoDto>> GetProjetoById(int id)
        {
            var projeto = await _context.Projetos
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProjetoInfoDto
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    Descricao = p.Descricao,
                    Tecnologias = p.Tecnologias,
                    LinkAcesso = p.LinkAcesso,
                    ImagemUrl = p.ImagemUrl,
                    VideoUrl = p.VideoUrl,
                    AlunoId = p.AlunoId,
                    AlunoNome = p.Aluno!.Nome
                })
                .FirstOrDefaultAsync();

            if (projeto == null)
            {
                return NotFound(new { message = "Projeto não encontrado" });
            }

            return Ok(projeto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] CriarProjetoDto dto)
        {
            var projeto = await _context.Projetos.FindAsync(id);

            if (projeto == null)
                return NotFound(new { message = "Projeto não encontrado" });

            if (string.IsNullOrWhiteSpace(dto.Titulo) ||
                string.IsNullOrWhiteSpace(dto.Descricao) ||
                string.IsNullOrWhiteSpace(dto.Tecnologias))
                return BadRequest(new { message = "Título, descrição e tecnologias são obrigatórios" });

            if (!await _context.Alunos.AnyAsync(a => a.Id == dto.AlunoId))
                return BadRequest(new { message = "O projeto deve pertencer a um aluno existente" });

            projeto.Titulo = dto.Titulo;
            projeto.Descricao = dto.Descricao;
            projeto.Tecnologias = dto.Tecnologias;
            projeto.LinkAcesso = dto.LinkAcesso;
            projeto.ImagemUrl = dto.ImagemUrl;
            projeto.VideoUrl = dto.VideoUrl;
            projeto.AlunoId = dto.AlunoId;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Projeto atualizado com sucesso" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var projeto = await _context.Projetos.FindAsync(id);

            if (projeto == null)
                return NotFound(new { message = "Projeto não encontrado" });

            _context.Projetos.Remove(projeto);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Projeto excluído com sucesso" });
        }
    }
}
