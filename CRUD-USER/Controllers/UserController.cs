using CRUD_USER.Data;
using CRUD_USER.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;                      
namespace CRUD_USER.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase //mostrar o controller da appi
    {
        private readonly AppDbContext _context; 
        public UsuariosController(AppDbContext context)
        {
            _context = context; //acessar o bd?? acho
        }

        [HttpPost]
        public async Task<IActionResult> Criar(User usuario)
        {
            _context.Users.Add(usuario);
            await _context.SaveChangesAsync();
            return Ok(usuario);
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
}
//pra cada função (Criar, Listar, BuscarPorEmail, Atualizar, Deletar) tem que ter um endpoint, rota,http
//post,get,delete,put