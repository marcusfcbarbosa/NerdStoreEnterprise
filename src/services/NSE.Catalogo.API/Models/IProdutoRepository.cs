using NSE.Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Catalogo.API.Models
{
    public interface IProdutoRepository : IRepository<Produto>
    {

        Task<PagedResult<Produto>> ObterTodosPaginado(int pageSize, int pageIndex,string query =null);
        Task<List<Produto>> ObterProdutosPorId(string ids);
    }
}
