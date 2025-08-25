using CRUD_USER.Validators;
using CRUD_USER.Data;
using CRUD_USER.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
namespace CRUD_USER.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuariosController : ControllerBase //mostrar o controller da appi
    {
        private readonly AppDbContext _context; 
        public UsuariosController(AppDbContext context)
        {
            _context = context; //acessar o bd?? acho
        }
        [HttpPost]
        public async Task<IActionResult> Create ([FromBody] CreateUserModel model)
            {
             await new CreateUserValidator(_context).ValidateAndThrowAsync(model); //validação do FluentValidation
            {
                
            }
            var usuario = new User
            {
                Nome = model.Nome,
                Email = model.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(model.Senha) //hashing da senha
            };
            _context.Users.Add(usuario);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Usuário registrado com sucesso." });
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> BuscarPorEmail(string email)
        {
            var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, User usuarioAtualizado)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null) return NotFound();

            usuario.Nome = usuarioAtualizado.Nome;
            usuario.Email = usuarioAtualizado.Email;

            await _context.SaveChangesAsync();
            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null) return NotFound();

            _context.Users.Remove(usuario);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }

    public class CreateUserModel
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
//pra cada função (Criar, Listar, BuscarPorEmail, Atualizar, Deletar) tem que ter um endpoint, rota,http
//post,get,delete,put