﻿using NSE.Core.DomainObjects;
using NSE.Pedidos.Domain.Vouchers.Enums;
using NSE.Pedidos.Domain.Vouchers.Specs;
using System;

namespace NSE.Pedidos.Domain.Vouchers
{
    public class Voucher : Entity, IAggregateRoot
    {
        public string Codigo { get; private set; }
        public decimal? Percentual { get; private set; }
        public decimal? ValorDesconto { get; private set; }
        public int Quantidade { get; private set; }
        public TipoDescontoVoucher TipoDesconto { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataUtilizacao { get; private set; }
        public DateTime DataValidade { get; private set; }
        public bool Ativo { get; private set; }
        public bool Utilizado { get; private set; }

        public bool EstaValidoParaUtilizacao()
        {
            return new VoucherAtivoSpecification()
                .And(new VoucherQuantidadeSpecification())
                .And(new VoucherDataSpecification()).IsSatisfiedBy(this);
        }
        public void MarcarComoUtilizado()
        {
            Ativo = false;
            Utilizado = true;
            Quantidade = 0;
            DataUtilizacao = DateTime.Now;
        }
        public void DebitarQuantidade()
        {
            Quantidade -= 1;
            if (Quantidade >= 1) return;

            MarcarComoUtilizado();
        }
    }
}
