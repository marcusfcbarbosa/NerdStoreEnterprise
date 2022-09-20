using Microsoft.EntityFrameworkCore;
using NSE.Cliente.API.Models;
using NSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Cliente.API.Data.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ClientesContext _context;
        public ClienteRepository(ClientesContext context)
        {
            _context = context;
        }
        public IUnitOfWork UnitOfWork => _context;

        public void Adicionar(Models.Cliente entity)
        {
            _context.Clientes.Add(entity);
        }

        public void AdicionarEndereco(Endereco endereco)
        {
            _context.Enderecos.Add(endereco);
        }

        public async Task Atualizar(Models.Cliente entity)
        {
            var client = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == entity.Id);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async Task<Endereco> ObterEnderecoPorId(Guid id)
        {
            return await _context.Enderecos.FirstOrDefaultAsync(e => e.ClienteId == id);
        }

        public async Task<Models.Cliente> ObterPorCpf(string cpf)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Cpf.Numero == cpf);
        }

        public Task<Models.Cliente> ObterPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Models.Cliente>> ObterTodos()
        {
            return await _context.Clientes.AsNoTracking().ToListAsync();
        }
    }
}
