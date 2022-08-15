﻿using MediatR;
using System;

namespace NSE.Core.Messages
{
    public class Event : Message, INotification
    {
        public DateTime TimeStamp { get; private set; }
        public Event()
        {
            TimeStamp = DateTime.Now;
        }

    }
}
