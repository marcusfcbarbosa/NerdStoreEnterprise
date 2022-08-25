using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
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
            switch ((int)response.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);

                case 400:
                    return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }

        protected ResponseResult RetornoOk()
        {
            return new ResponseResult();
        }
    }
}
