using EasyNetQ;
using NSE.Core.Messages.Integration;
using Polly;
using RabbitMQ.Client.Exceptions;
using System;
using System.Threading.Tasks;

namespace NSE.MessageBus
{
    public interface IMessageBus : IDisposable
    {
        bool IsConnected { get; }
        IAdvancedBus AdvancedBus { get; }

        void Publish<T>(T message) where T : IntegrationEvent;

        Task PublishAsync<T>(T message) where T : IntegrationEvent;

        void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class;

        void SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : class;

        TResponse Request<TRequest, TResponse>(TRequest request)
            where TRequest : IntegrationEvent
            where TResponse : ResponseMessage;

        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
            where TRequest : IntegrationEvent
            where TResponse : ResponseMessage;

        IDisposable Respond<TRequest, TResponse>(Func<TRequest, TResponse> responder)
            where TRequest : IntegrationEvent
            where TResponse : ResponseMessage;

        IDisposable RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder)
            where TRequest : IntegrationEvent
            where TResponse : ResponseMessage;
    }
    public class MessageBus : IMessageBus
    {
        public IBus _bus;
        private readonly string _connectionString;

        public MessageBus(string connectionString)
        {
            _connectionString = connectionString;
            TryConnect();
        }
        private void TryConnect()
        {
            if (IsConnected) return;

            var policy = Policy.Handle<EasyNetQException>()//politica para erros do EasyNetQ
                .Or<BrokerUnreachableException>() //Politica para erros do RAbbitMq
                .WaitAndRetry(3, retryAttempt => //3 tentativas e para cada tentativa espere 2,4,8 segundos
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            policy.Execute(action: () => { _bus = RabbitHutch.CreateBus(_connectionString); });

            _bus = RabbitHutch.CreateBus(_connectionString);
        }

        public bool IsConnected => _bus?.IsConnected ?? false;

        public IAdvancedBus AdvancedBus => throw new NotImplementedException();
        public void Dispose()
        {
            _bus.Dispose();
        }

        public void Publish<T>(T message) where T : IntegrationEvent
        {
            TryConnect();
            _bus.Publish(message);
        }

        public async Task PublishAsync<T>(T message) where T : IntegrationEvent
        {
            TryConnect();
            await _bus.PublishAsync(message);
        }
        public void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class
        {
            TryConnect();
            _bus.Subscribe(subscriptionId, onMessage);
        }

        public void SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : class
        {
            TryConnect();
            _bus.SubscribeAsync(subscriptionId, onMessage);
        }
        public TResponse Request<TRequest, TResponse>(TRequest request)
            where TRequest : IntegrationEvent
            where TResponse : ResponseMessage
        {
            TryConnect();
            return _bus.Request<TRequest, TResponse>(request);
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
            where TRequest : IntegrationEvent
            where TResponse : ResponseMessage
        {
            TryConnect();
            return await _bus.RequestAsync<TRequest, TResponse>(request);
        }

        public IDisposable Respond<TRequest, TResponse>(Func<TRequest, TResponse> responder)
            where TRequest : IntegrationEvent
            where TResponse : ResponseMessage
        {
            TryConnect();
            return _bus.Respond(responder);
        }

        public IDisposable RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder)
            where TRequest : IntegrationEvent
            where TResponse : ResponseMessage
        {
            TryConnect();
            return _bus.RespondAsync(responder);
        }


    }
}
