using Microsoft.AspNetCore.Mvc;
using CRUD_USER.Data;
using CRUD_USER.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FluentValidation;
using CRUD_USER.Validators;

namespace CRUD_USER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext context) //mesma coisa do UserController, injetando o contexto do banco de dados
        {
            _context = context;
        }
        [HttpPost]   //agora é uma grande cópia do UserController
        public async Task<IActionResult> Criar( [FromBody] CreateProdutoModel model)
        {
            await new CreateProdutoValidator(_context).ValidateAndThrowAsync(model); //validação do FluentValidation

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null) return Unauthorized("Usuário não autenticado"); //se n tiver o id do user, n ta autenticado

            // Busque o Fornecedor no banco de dados, já que ele é um campo obrigatório
            var fornecedor = await _context.Fornecedores.FindAsync(model.IdFornecedor);
            if (fornecedor == null) return BadRequest("O fornecedor não existe."); //se n achar o fornecedor, retorna badrequest

            // Busque o Usuario Criador no banco de dados para a propriedade obrigatória
            var usuarioCriador = await _context.Users.FindAsync(int.Parse(userIdString));
            if (usuarioCriador == null) return Unauthorized("Usuário criador não encontrado."); //se n achar o user, retorna unauthorized

            var produto = new Produto
            {
                Nome = model.Nome,
                Descricao = model.Descricao,
                Preco = model.Preco,
                Fornecedor = fornecedor,
                DataCriacao = DateTime.Now,
                // E o ID do usuário que cria o produto
                UsuarioCriador = usuarioCriador
            };
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return Ok(produto);
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
    public class CreateProdutoModel
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; } 
        public int IdFornecedor { get; set; } 
    }
}
//no fim das contas é tudo igual ao UserController, só muda as propriedades do produto e adiciona o fornecedor e o usuario criador como relacionamento (fkey)