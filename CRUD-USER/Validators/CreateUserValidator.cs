using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CRUD_USER.Data;
using CRUD_USER.Entities;
using CRUD_USER.Controllers;
using FluentValidation;
namespace CRUD_USER.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserModel>
    {
        private readonly AppDbContext _context;
        public CreateUserValidator(AppDbContext context)
        {
            _context = context;
            RuleFor(user => user.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(100).WithMessage("O nome não pode exceder 100 caracteres.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("O email deve ser um endereço válido.")
                .MaximumLength(100).WithMessage("O email não pode exceder 100 caracteres.")
                .MustAsync(async (email, cancellation) =>
                {
                   var emailExists = await _context.Users.AnyAsync(u => u.Email == email, cancellation);
                    return !emailExists;
                }).WithMessage("O email já está em uso.");

            RuleFor(user => user.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.")
                .MaximumLength(100).WithMessage("A senha não pode exceder 100 caracteres.");
        }

    }
}
