using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public interface ICatalogoService {

        Task<IEnumerable<ProdutoViewModel>> ObterTodos();
        Task<ProdutoViewModel> ObterPorId(Guid id);

    }
    public class CatalogoService : Service, ICatalogoService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _settings;
        public CatalogoService(HttpClient httpClient,
            IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
            
        }
        public async Task<ProdutoViewModel> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync(requestUri: $"/catalogo/produtos/{id}");

            TratarErrosResponse(response);
            return await DeserializarObjetoResponse<ProdutoViewModel>(response);
        }

        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            var response = await _httpClient.GetAsync(requestUri: $"/catalogo/produtos");
            TratarErrosResponse(response);
            return await DeserializarObjetoResponse<IEnumerable<ProdutoViewModel>>(response);
        }
    }

}
