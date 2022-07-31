using NSE.WebApp.MVC.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public interface IAutenticacaoService
    {
        Task<T> Login<T>(UsuarioLogin usuarioLogin) where T : class;
        Task<T> Registro<T>(UsuarioRegistro usuarioRegistro) where T : class;
    }

    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly HttpClient _httpClient;
        public AutenticacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<T> Login<T>(UsuarioLogin usuarioLogin) where T : class
        {
            try
            {
                var loginContent = new StringContent(JsonSerializer.Serialize(usuarioLogin), encoding: Encoding.UTF8, mediaType: "application/json");
                var response = await _httpClient.PostAsync(requestUri: "https://localhost:44307/api/identidade/autenticar", content: loginContent);
                
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), options);
            }
            catch (Exception ex)
            {
                //TO DO capturar esse erro em log
                return default(T);
            }
        }

        public async Task<T> Registro<T>(UsuarioRegistro usuarioRegistro) where T : class
        {
            try
            {
                var registroContent = new StringContent(JsonSerializer.Serialize(usuarioRegistro), encoding: Encoding.UTF8, mediaType: "application/json");
                var response = await _httpClient.PostAsync(requestUri: "https://localhost:44307/api/identidade/nova-conta", content: registroContent);
                return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {//TO DO capturar esse erro em log
                return default(T);
            }
        }
    }
}