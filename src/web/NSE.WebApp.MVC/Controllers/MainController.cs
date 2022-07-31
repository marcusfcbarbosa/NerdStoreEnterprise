using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using System.Linq;

namespace NSE.WebApp.MVC.Controllers
{
    public class MainController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult resposta)
        {
            if(resposta != null && resposta.Errors.Messages.Any())
            {
                resposta.Errors.Messages.ForEach(x => ModelState.AddModelError(key:string.Empty,errorMessage: x));
                return true;
            }
            return false;
        }
    }
}
