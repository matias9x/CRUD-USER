using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CRUD_USER.Data;
using CRUD_USER.Entities;
using CRUD_USER.Controllers;
using FluentValidation;
namespace CRUD_USER.Validators
{
        public class CreateProdutoValidator : AbstractValidator<CreateProdutoModel>
        {
            private readonly AppDbContext _context;

            public CreateProdutoValidator(AppDbContext context)
            {
                // regras para o nome
                _context = context;
                RuleFor(p => p.Nome)
                    .NotEmpty().WithMessage("O nome do produto é obrigatório.")
                    .MaximumLength(150).WithMessage("O nome não pode exceder 150 caracteres.")
                .MustAsync(async (nome, cancellation) =>
                 {
                     // A validação para nome único acontece aqui
                     var nomeExists = await _context.Produtos.AnyAsync(p => p.Nome == nome, cancellation);
                     return !nomeExists;
                 }).WithMessage("Este nome de produto já está em uso.");


                // regras para a descrição
                RuleFor(p => p.Descricao)
                    .NotEmpty().WithMessage("A descrição do produto é obrigatória.")
                    .MaximumLength(500).WithMessage("A descrição não pode exceder 500 caracteres.");

                // regras para o preço
                RuleFor(p => p.Preco)
                    .NotEmpty().WithMessage("O preço do produto é obrigatório.");

              // regras para o fornecedor
                 RuleFor(p => p.IdFornecedor)
                    .NotEmpty().WithMessage("O ID do fornecedor é obrigatório.")
                    .MustAsync(async (idFornecedor, cancellation) =>
                   {
                     var fornecedorExists = await _context.Fornecedores.AnyAsync(f => f.IdFornecedor == idFornecedor, cancellation);
                     return fornecedorExists;
                     }).WithMessage("O fornecedor com o ID fornecido não foi encontrado.");

        }
        }
    }

