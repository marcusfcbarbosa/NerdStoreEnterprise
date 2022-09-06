﻿using FluentValidation.Results;
using NSE.Core.Messages;
using System.Threading.Tasks;

namespace NSE.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task<ValidationResult> EnviarCommando<T>(T commando) where T : Command;
    }
}