using NSE.WebApp.MVC.Extensions;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public abstract class Service
    {

        protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage response)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), options);
        }

        protected StringContent ObterConteudo(object dado)
        {
            return new StringContent(JsonSerializer.Serialize(dado), encoding: Encoding.UTF8, mediaType: "application/json");
        }

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
