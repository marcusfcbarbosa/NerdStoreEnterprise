using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace NSE.Core.Messages
{
    public abstract class Command : Message, IRequest<ValidationResult>
    { 
        public DateTime TimeStamp { get; private set; }

        public ValidationResult validationResult { get;  set; }
        public Command()
        {
            TimeStamp = DateTime.Now;
        }

        public virtual bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}
