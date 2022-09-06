﻿using NSE.Core.Data;
using NSE.Pedidos.Domain.Vouchers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NSE.Pedidos.Infra.Data.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly PedidosContext _context;
        public VoucherRepository(PedidosContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
