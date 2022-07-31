using NSE.WebApp.MVC.Extensions;
using System;
using System.Net.Http;

namespace NSE.WebApp.MVC.Services
{
    public abstract class Service
    {
        protected bool TratarErrosResponse(HttpResponseMessage response)
        {
            return (int)response.StatusCode switch
            {
                400 => false,
                401 => false,
                403 => false,
                404 => false,
                500 => throw new CustomHttpRequestException(response.StatusCode),
                _ => (((Func<bool>)(() =>
               {
                   response.EnsureSuccessStatusCode();
                   return true;
               }))())
            };
        }
    }
}
