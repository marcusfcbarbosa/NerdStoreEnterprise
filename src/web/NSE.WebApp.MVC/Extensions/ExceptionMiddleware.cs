using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Polly.CircuitBreaker;
using Refit;

namespace NSE.WebApp.MVC.Extensions
{
    public class ExceptionMiddleware
    {

        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (CustomHttpRequestException ex)
            {
                HandleRequestExceptionAsyn(httpContext,ex);
            }catch(ValidationApiException ex)
            {
                HandleRequestExceptionAsyn(httpContext, ex.StatusCode);
            }
            catch (BrokenCircuitException)
            {
                HandleCircuitBreakerExceptionAsync(httpContext);
            }
        }

        //todo erro que acontece passa por aqui
        private static void HandleRequestExceptionAsyn(HttpContext context, CustomHttpRequestException httpRequestException)
        {
            switch (httpRequestException._statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");//ReturnUrl=Pega a rota que estava antes de ter gerado a Exception (de onde vc estava vindo)
                    return;
            }
            context.Response.StatusCode = (int)httpRequestException._statusCode;
        }
        //adequado para trabalhar com refit
        private static void HandleRequestExceptionAsyn(HttpContext context, HttpStatusCode httpStatusCode)
        {
            switch (httpStatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");//ReturnUrl=Pega a rota que estava antes de ter gerado a Exception (de onde vc estava vindo)
                    return;
            }
            context.Response.StatusCode = (int)httpStatusCode;
        }
        private static void HandleCircuitBreakerExceptionAsync(HttpContext context)
        {

            context.Response.Redirect(location: "/erro/503");
        }
    }
}
