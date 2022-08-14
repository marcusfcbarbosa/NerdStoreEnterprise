
using FluentValidation.Results;
using MediatR;
using NSE.Cliente.API.Models;
using NSE.Core.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Cliente.API.Application.Commands
{
    public class ClienteCommandHandler : CommandHandler,
                IRequestHandler<RegistrarClientCommand, ValidationResult>
    {
        public async Task<ValidationResult> Handle(RegistrarClientCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.validationResult;

            var idCliente = Guid.NewGuid();
            var cliente = new Clientes(idCliente, message.Nome, message.Email, message.Cpf);
            if (true) { //Ja existe cliente com cpf informado

                AdicionarErro("Cpf ja esta em uso");
                return _validationResult;
            }


            return message.validationResult;
        }

        
    }
}
