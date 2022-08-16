using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.Handlers;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System;
using System.Net.Http;

namespace NSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();

            //Primeiro tipo de HttpClientFactory usando Retry com Polly
            //services.AddHttpClient<ICatalogoService, CatalogoService>()
            //    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            //    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: _ => TimeSpan.FromMilliseconds(600)));

            //Segunda ipo de HttpClientFactory usando Retry com Polly
            services.AddHttpClient<ICatalogoService, CatalogoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AddTransientHttpErrorPolicy
                (p=>p.CircuitBreakerAsync(handledEventsAllowedBeforeBreaking:5,TimeSpan.FromSeconds(30)));//após 5 tentativas, corta e só fecha depois de 30 segundos sem acessar qualquer rota dentro da aplicaçao

            services.AddHttpClient(name: "Refit", configureClient: options =>
            {
                options.BaseAddress = new Uri(uriString: configuration.GetSection(key: "CatalogoUrl").Value);
            }).AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
        }
    }
    public class PollyExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> EsperarTentar()
        {
            //Segunda forma de HttpClientFactory usando Retry com Polly usando algum tipo de politica
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(sleepDurations: new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(1),

                }, onRetry: (outcome, timespan, retryCount, context) => {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(value: $"Tentando pela {retryCount} vez");
                    Console.ForegroundColor = ConsoleColor.White;
                });

        }
    }
}

