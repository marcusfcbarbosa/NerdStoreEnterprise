using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using System.Linq;

namespace NSE.WebApp.MVC.Controllers
{
    public class MainController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult resposta)
        {
            if(resposta?.Errors != null && resposta.Errors.Mensagens.Any())
            {
                resposta.Errors.Mensagens.ForEach(x => ModelState.AddModelError(key:string.Empty,errorMessage: x));
                return true;
            }
            return false;
        }
        protected void AdicionarErroValidacao(string mensagem)
        {
            ModelState.AddModelError(string.Empty, mensagem);
        }

        protected bool OperacaoValida()
        {
            return ModelState.ErrorCount == 0;
        }
    }
}
