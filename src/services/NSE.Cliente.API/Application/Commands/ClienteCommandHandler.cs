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
                IRequestHandler<RegistrarClientCommand, ValidationResult>,
                IRequestHandler<AdicionarEnderecoCommand, ValidationResult>
    {
        private readonly IClienteRepository _clienteRepository;
        public ClienteCommandHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ValidationResult> Handle(RegistrarClientCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.validationResult;
            var cliente = new Models.Cliente(message.Id, message.Nome, message.Email, message.Cpf);
            //validação
            var previousClient = await _clienteRepository.ObterPorCpf(cliente.Cpf.Numero);
            if (previousClient != default(Models.Cliente)) {
                AdicionarErro("Cpf ja esta em uso");
                return _validationResult;
            }
            _clienteRepository.Adicionar(cliente);
            //Após o cliente Criado disparar Evento de boas vindas ou quaisquer coisas que o EventHandler tratar
            cliente.AdicionarEvento(new ClienteRegistradoEvent(message.Id, message.Nome, message.Email, message.Cpf));
            return await PersistirDados(_clienteRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(AdicionarEnderecoCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.validationResult;

            var endereco = new Endereco(message.Logradouro, message.Numero,
                                        message.Complemento, message.Bairro, 
                                        message.Cep, message.Cidade, message.Estado,
                                        message.ClienteId);

            _clienteRepository.AdicionarEndereco(endereco);

            return await PersistirDados(_clienteRepository.UnitOfWork);
        }
    }
}
