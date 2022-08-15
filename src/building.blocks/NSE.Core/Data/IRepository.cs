using NSE.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot //Apenas um repositorio por agregação
    {
        Task<IEnumerable<T>> ObterTodos();
        Task<T> ObterPorId(Guid id);
        void Adicionar(T entity);
        Task Atualizar(T entity);
    }


}
