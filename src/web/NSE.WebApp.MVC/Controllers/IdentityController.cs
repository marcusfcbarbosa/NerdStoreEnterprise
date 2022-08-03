using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class IdentityController : MainController
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public IdentityController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro()
        {

            return View();
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return View(usuarioRegistro);

            var resposta = await _autenticacaoService.Registro(usuarioRegistro);

            if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioRegistro);

            await RealizarLogin(resposta);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UsuarioLogin usuarioLogin, string returnUrl = null)
        {
            
            if (!ModelState.IsValid) return View(usuarioLogin);
            //API Registro
            var result = await _autenticacaoService.Login(usuarioLogin);
            if(!ResponsePossuiErros(result.ResponseResult))
            {
                //Salvando os dados do JWT no Cookie da aplicação
                await RealizarLogin(result);
                if (string.IsNullOrEmpty(returnUrl))
                  return RedirectToAction(actionName: "Index", controllerName: "Home");
                        
                 return LocalRedirect(returnUrl);
            }
            return View(usuarioLogin);
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); 
            return RedirectToAction("Index", "Home");
        }


        private  async Task RealizarLogin(UsuarioRespostaLogin usuarioRespostaLogin)
        {
            //irá trabalhar no esquema de autenticação usando Cookies
            var token = ObterTokenFormatado(usuarioRespostaLogin.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", usuarioRespostaLogin.AccessToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true//dura multiplos requests
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
        private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }

    }
}
