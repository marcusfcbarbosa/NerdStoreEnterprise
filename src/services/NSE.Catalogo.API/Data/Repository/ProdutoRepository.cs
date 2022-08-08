﻿using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.API.Models;
using NSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Catalogo.API.Data.Repository
{
    /// <summary>
    /// Nao fazer Commit pelo Repositorio, usar IUnitOfWork
    /// </summary>
    public class Produtorepository : IProdutoRepository
    {
        private readonly CatalogoContext  _context;
        
        public Produtorepository(CatalogoContext context)
        {
            _context = context;
        }
        public IUnitOfWork IUnitOfWork => _context;
        public async Task<IEnumerable<Produto>> ObterTodos()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }

        public async Task<Produto> ObterPorId(Guid id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public void Adicionar(Produto produto)
        {
            _context.Produtos.Add(produto);
        }

        public void Atualizar(Produto produto)
        {
            _context.Produtos.Update(produto);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
