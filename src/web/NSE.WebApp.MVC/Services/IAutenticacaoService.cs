using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Extensions;
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
        Task<T> Login<T>(UsuarioLogin usuarioLogin) where T : class;//Precisa pensar num formato para ter retorno das mensagens em Generic
        Task<T> Registro<T>(UsuarioRegistro usuarioRegistro) where T : class;//Precisa pensar num formato para ter retorno das mensagens em Generic
        Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin);
        Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro);
    }
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _settings;//ler dos arquivos appSettings.Developement
        public AutenticacaoService(HttpClient httpClient,
            IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.AutenticacaoUrl);
            
        }
        public async Task<T> Login<T>(UsuarioLogin usuarioLogin) where T : class
        {
            try
            {
                var loginContent = ObterConteudo(usuarioLogin);
                var response = await _httpClient.PostAsync(requestUri: "https://localhost:44307/api/identidade/autenticar", content: loginContent);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                if (!TratarErrosResponse(response))
                {
                    //Nesse formato ainda nao pega as mensagens de retorno
                }
                return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), options);
            }
            catch (Exception ex)
            {
                //TO DO capturar esse erro em log
                return default(T);
            }
        }
        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
        {
            var loginContent = ObterConteudo(usuarioLogin);
            var response = await _httpClient.PostAsync(requestUri: $"/api/identidade/autenticar", content: loginContent);
            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }
            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }

        public async Task<T> Registro<T>(UsuarioRegistro usuarioRegistro) where T : class
        {

            var registroContent = ObterConteudo(usuarioRegistro);
            var response = await _httpClient.PostAsync(requestUri: "/api/identidade/nova-conta", content: registroContent);
            return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync());
        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
        {
            var registroContent = ObterConteudo(usuarioRegistro);
            var response = await _httpClient.PostAsync(requestUri: $"/api/identidade/nova-conta", content: registroContent);
            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }
            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }
    }
}