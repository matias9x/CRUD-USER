using Microsoft.AspNetCore.Mvc;
using CRUD_USER.Data;
using CRUD_USER.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CRUD_USER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProdutosController : Controller
    {
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext context) //mesma coisa do UserController, injetando o contexto do banco de dados
        {
            _context = context;
        }
        [HttpPost]   //agora é uma grande cópia do UserController
        public async Task<IActionResult> Criar(Produto produto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //pegar o id do usuario logado do token JWT
            if (userId == null) return Unauthorized("Usuário não autenticado"); //se n tiver o id do usuario retorna 401

            var userIdInt = int.Parse(userId); //converter o id do usuario pra int pq a fkey é int
            produto.IdUsuarioCriador = int.Parse(userId); //setar o id do usuario no produto
            produto.IdUsuarioCriador = userIdInt; //setar o id do usuario criador do produto
            produto.DataCriacao = DateTime.Now; //setar a data de criação do produto;

            _context.Produtos.Add(produto); //adicionar o produto ao contexto
            await _context.SaveChangesAsync(); //salva o produto no banco de dados
            return Ok(produto); //retorna o produto criado
        }
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _context.Produtos
                .Include(produto => produto.Fornecedor) //incluir o fornecedor na consulta
                .Include(produto => produto.UsuarioCriador) //incluir o usuario criador na consulta
                .ToListAsync()); //retorna a lista de produtos com o fornecedor e o usuario criador
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var produto = await _context.Produtos
                .Include(p => p.Fornecedor) //incluir o fornecedor na consulta, se colocar só o p ele entende que é produto
                .Include(p => p.UsuarioCriador) //incluir o usuario criador na consulta
                .FirstOrDefaultAsync(p => p.IdProduto == id); //achar o produto pelo id
            if (produto == null) return NotFound(); //se n achar retorna 404
            return Ok(produto); //se achar retorna o produto
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Produto produtoAtualizado)
        {
            var produto = await _context.Produtos.FindAsync(id); //achar o produto pelo id
            if (produto == null) return NotFound(); //se n achar retorna 404
            produto.Nome = produtoAtualizado.Nome; //atualizar os campos do produto, no caso nome
            produto.Descricao = produtoAtualizado.Descricao; //atualizar a descrição
            produto.Preco = produtoAtualizado.Preco;//atualizar o preço
            produto.IdFornecedor = produtoAtualizado.IdFornecedor; //atualizar o fornecedor
            await _context.SaveChangesAsync(); //salvar as mudanças
            return Ok(produto); //retornar o produto atualizado
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var produto = await _context.Produtos.FindAsync(id); //achar o produto pelo id
            if (produto == null) return NotFound(); //se n achar retorna 404
            _context.Produtos.Remove(produto); //remover o produto
            await _context.SaveChangesAsync(); //salvar as mudanças
            return Ok();
        }
    }
}
//no fim das contas é tudo igual ao UserController, só muda as propriedades do produto e adiciona o fornecedor e o usuario criador como relacionamento (fkey)