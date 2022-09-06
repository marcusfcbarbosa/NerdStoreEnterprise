using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Extensions
{
    public class CarrinhoViewComponent : ViewComponent
    {

        private readonly IComprasBFFService _carrinhoService;

        public CarrinhoViewComponent(IComprasBFFService carrinhoService)
        {
            _carrinhoService = carrinhoService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _carrinhoService.ObterQuantidadeCarrinho());
        }

    }
}
