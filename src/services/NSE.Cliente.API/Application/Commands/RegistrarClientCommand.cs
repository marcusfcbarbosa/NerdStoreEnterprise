using FluentValidation;
using NSE.Core.Messages;
using System;

namespace NSE.Cliente.API.Application.Commands
{
    public class RegistrarClientCommand : Command
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }

        public RegistrarClientCommand(string nome, string email, string cpf)
        {
            AggregateId = Guid.NewGuid();
            Nome = nome;
            Email = email;
            Cpf = cpf;
        }
        public override bool EhValido()
        {
            validationResult = new RegistrarClientValidation().Validate(instance:this);
            return validationResult.IsValid;
        }
    }

    public class RegistrarClientValidation : AbstractValidator<RegistrarClientCommand>
    {
        public RegistrarClientValidation()
        {
            RuleFor(c => c.Nome)
                .NotEqual(string.Empty)
                .WithMessage("Nome Inválido");
            
            RuleFor(c => c.Cpf)
                .Must(CpfValido)
                .WithMessage("Cpf Inválido");

            RuleFor(c => c.Email)
                .Must(EmailValido)
                .WithMessage("EmailValido Inválido");
        }
        protected static bool CpfValido(string cpf)
        {
            return Core.DomainObjects.Cpf.Validar(cpf);
        }
        protected static bool EmailValido(string email)
        {
            return Core.DomainObjects.Email.Validar(email);
        }
    }
}
