using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSE.Identity.API.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace NSE.Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class MainController : Controller
    {
        protected ICollection<string> Errors = new List<string>();
        protected ActionResult CustomReponse(object result = null)
        {
            if (OperacaoValida()) return Ok(result);
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]> {//padrao implementado numa RFC
                { "Messages", Errors.ToArray() }
            }));
        }
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            erros.ForEach(e => AdicionarErrosProcessamento(e.ErrorMessage));
            return CustomReponse();
        }

        protected bool OperacaoValida()
        {
            return !Errors.Any();
        }
        protected void AdicionarErrosProcessamento(string erro)
        {
            Errors.Add(erro);
        }
        protected void LimpaErrosProcessamento()
        {
            Errors.Clear();
        }
    }
}