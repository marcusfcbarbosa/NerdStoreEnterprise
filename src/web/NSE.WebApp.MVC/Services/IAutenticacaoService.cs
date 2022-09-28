using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using NSE.Core.Communications;
using NSE.WebApi.Core.Usuarios;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public interface IAutenticacaoService
    {
        //Task<T> Login<T>(UsuarioLogin usuarioLogin) where T : class;//Precisa pensar num formato para ter retorno das mensagens em Generic
        //Task<T> Registro<T>(UsuarioRegistro usuarioRegistro) where T : class;//Precisa pensar num formato para ter retorno das mensagens em Generic
        Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin);
        Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro);
        Task RealizarLogin(UsuarioRespostaLogin resposta);
        Task Logout();
        bool TokenExpirado();
        Task<bool> RefreshTokenValido();
    }
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;
        private readonly IAspNetUser _user;//para trabalhar com dados do usuario logado
        private readonly IAuthenticationService _authenticationService; //Através dessa interface se manipula Login e Logout 
        public AutenticacaoService(HttpClient httpClient,
                                IOptions<AppSettings> settings,
                                IAspNetUser user, IAuthenticationService authenticationService)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.AutenticacaoUrl);

            _user = user;
            _authenticationService = authenticationService;
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
        public async Task Logout()
        {
            await _authenticationService.SignOutAsync(
                _user.ObterHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                null);
        }

        public async Task RealizarLogin(UsuarioRespostaLogin resposta)
        {
            var token = ObterTokenFormatado(resposta.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", resposta.AccessToken));
            claims.Add(new Claim("RefreshToken", resposta.RefreshToken));//inseri mais uma claim de refresh token
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                IsPersistent = true
            };

            await _authenticationService.SignInAsync(
                _user.ObterHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public async Task<bool> RefreshTokenValido()
        {
            var resposta = await UtilizarRefreshToken(_user.ObterUserRefreshToken());

            if (resposta.AccessToken != null && resposta.ResponseResult == null)
            {
                await RealizarLogin(resposta);
                return true;
            }

            return false;
        }
        public async Task<UsuarioRespostaLogin> UtilizarRefreshToken(string refreshToken)
        {
            var refreshTokenContent = ObterConteudo(refreshToken);

            var response = await _httpClient.PostAsync("/api/identidade/refresh-token", refreshTokenContent);

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }
        public bool TokenExpirado()
        {
            var jwt = _user.ObterUserToken();
            if (jwt is null) return false;

            var token = ObterTokenFormatado(jwt);
            return token.ValidTo.ToLocalTime() < DateTime.Now;
        }

        public static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }

        //public async Task<T> Registro<T>(UsuarioRegistro usuarioRegistro) where T : class
        //{

        //    var registroContent = ObterConteudo(usuarioRegistro);
        //    var response = await _httpClient.PostAsync(requestUri: "/api/identidade/nova-conta", content: registroContent);
        //    return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync());
        //}

        //public async Task<T> Login<T>(UsuarioLogin usuarioLogin) where T : class
        //{
        //    try
        //    {
        //        var loginContent = ObterConteudo(usuarioLogin);
        //        var response = await _httpClient.PostAsync(requestUri: "https://localhost:44307/api/identidade/autenticar", content: loginContent);
        //        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        //        if (!TratarErrosResponse(response))
        //        {
        //            //Nesse formato ainda nao pega as mensagens de retorno
        //        }
        //        return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), options);
        //    }
        //    catch (Exception ex)
        //    {
        //        //TO DO capturar esse erro em log
        //        return default(T);
        //    }
        //}
    }
}