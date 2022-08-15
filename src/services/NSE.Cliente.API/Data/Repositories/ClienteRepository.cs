using Microsoft.EntityFrameworkCore;
using NSE.Cliente.API.Models;
using NSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Cliente.API.Data.Repositories
{
    public interface IClienteRepository : IRepository<Clientes>
    {
        IUnitOfWork unitOfWork { get; }
        Task<Clientes> ObterPorCpf(string cpf);
    }
    public class ClienteRepository : IClienteRepository
    {
        private readonly ClientesContext _context;
        public ClienteRepository(ClientesContext context)
        {
            _context = context;
        }
        public IUnitOfWork unitOfWork => _context;
        public void Adicionar(Clientes entity)
        {
            _context.Clientes.Add(entity);
        }

        public async Task Atualizar(Clientes entity)
        {
            var client = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == entity.Id);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async Task<Clientes> ObterPorCpf(string cpf)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Cpf.Numero == cpf);
        }

        public Task<Clientes> ObterPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Clientes>> ObterTodos()
        {
            return await _context.Clientes.AsNoTracking().ToListAsync();
        }
    }
}
