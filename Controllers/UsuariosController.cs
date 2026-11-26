using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoFinalPortfolio.Data;
using ProjetoFinalPortfolio.models;
using ProjetoFinalPortfolio.models.DTOs;

namespace ProjetoFinalPortfolio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Usuario> _hasher = new();

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CriarUsuarioDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Senha))
                return BadRequest(new { message = "Nome, e-mail e senha são obrigatórios" });

            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return Conflict(new { message = "Já existe um usuário com esse e-mail" });

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email
            };
            usuario.Senha = _hasher.HashPassword(usuario, dto.Senha);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário criado com sucesso", id = usuario.Id });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioInfoDto>>> GetUsuarios([FromQuery] string? nome)
        {
            var usuarios = await _context.Usuarios
                .AsNoTracking()
                .Where(u => string.IsNullOrEmpty(nome) || u.Nome.Contains(nome))
                .OrderBy(u => u.Nome)
                .Select(u => new UsuarioInfoDto
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioInfoDto>> GetUsuarioById(int id)
        {
            var usuario = await _context.Usuarios
                .AsNoTracking()
                .Where(u => u.Id == id)
                .Select(u => new UsuarioInfoDto
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            return Ok(usuario);
        }

    
        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] CriarUsuarioDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(new { message = "Usuário não encontrado" });

            if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Nome e e-mail são obrigatórios" });

            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id != id))
                return Conflict(new { message = "Já existe um usuário com esse e-mail" });

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Senha))
                usuario.Senha = _hasher.HashPassword(usuario, dto.Senha);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário atualizado com sucesso" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(new { message = "Usuário não encontrado" });

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário excluído com sucesso" });
        }
    }
}
