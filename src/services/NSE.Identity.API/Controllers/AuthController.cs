using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSE.Identity.API.Models;
using System.Threading.Tasks;


namespace NSE.Identity.API.Controllers
{
    [Route("api/identidade")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;///gerencia questoes de login  classes do proprio Identity
        private readonly UserManager<IdentityUser> _userManager;//Como administra o usuario       |classes do proprio Identity
        //private readonly AppSettings _appSettings;


        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var resulst = await _signInManager.PasswordSignInAsync(userName: usuarioLogin.Email, password: usuarioLogin.Senha, isPersistent: false, lockoutOnFailure: true);
            if (resulst.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
