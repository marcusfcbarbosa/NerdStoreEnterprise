using NSE.Core.Data;
using System;
using System.Threading.Tasks;

namespace NSE.Cliente.API.Models
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Cliente> ObterPorCpf(string cpf);
        void AdicionarEndereco(Endereco endereco);
        Task<Endereco> ObterEnderecoPorId(Guid id);
    }
}
