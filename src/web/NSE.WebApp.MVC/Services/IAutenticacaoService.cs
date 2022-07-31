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
            try
            {
                var loginContent = new StringContent(JsonSerializer.Serialize(usuarioLogin), encoding: Encoding.UTF8, mediaType: "application/json");
                var response = await _httpClient.PostAsync(requestUri: "https://localhost:44307/api/identidade/autenticar", content: loginContent);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                if (!TratarErrosResponse(response))
                {
                    return new UsuarioRespostaLogin
                    {
                        ResponseResult = JsonSerializer.Deserialize<ResponseResult>(await response.Content.ReadAsStringAsync(), options)
                    };
                }
                
                return JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync(), options);
            }
            catch (Exception ex)
            {
                //TO DO capturar esse erro em log
                return default(UsuarioRespostaLogin);
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

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
        {
            try
            {
                var registroContent = new StringContent(JsonSerializer.Serialize(usuarioRegistro), encoding: Encoding.UTF8, mediaType: "application/json");
                var response = await _httpClient.PostAsync(requestUri: "https://localhost:44307/api/identidade/nova-conta", content: registroContent);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                if (!TratarErrosResponse(response))
                {
                    return new UsuarioRespostaLogin
                    {
                        ResponseResult = JsonSerializer.Deserialize<ResponseResult>(await response.Content.ReadAsStringAsync(), options)
                    };
                }
                return JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {//TO DO capturar esse erro em log
                return default(UsuarioRespostaLogin);
            }
        }
    }
}