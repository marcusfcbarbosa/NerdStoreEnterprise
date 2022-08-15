using FluentValidation.Results;
using MediatR;
using NSE.Cliente.API.Application.Events;
using NSE.Cliente.API.Data.Repositories;
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
        private readonly IClienteRepository _clienteRepository;
        public ClienteCommandHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ValidationResult> Handle(RegistrarClientCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.validationResult;

            var idCliente = Guid.NewGuid();
            var cliente = new Clientes(idCliente, message.Nome, message.Email, message.Cpf);
            //validação
            var previousClient = await _clienteRepository.ObterPorCpf(cliente.Cpf.Numero);
            if (previousClient != default(Clientes)) { 
                AdicionarErro("Cpf ja esta em uso");
                return _validationResult;
            }
            _clienteRepository.Adicionar(cliente);
            //Após o cliente Criado disparar Evento de boas vindas ou quaisquer coisas que o EventHandler tratar
            cliente.AdicionarEvento(new ClienteRegistradoEvent(idCliente, message.Nome, message.Email, message.Cpf));
            return await PersistirDados(_clienteRepository.unitOfWork);
        }

        
    }
}
