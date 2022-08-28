using Microsoft.AspNetCore.Mvc;
using NSE.BFF.Compras.Services;
using NSE.WebApi.Core.Controllers;
using System.Threading.Tasks;

namespace NSE.BFF.Compras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrinhoController : MainController
    {

        private readonly ICarrinhoService _carrinhoService;
        private readonly ICatalogoService _catalogoService;

        public CarrinhoController(ICarrinhoService carrinhoService
                                 ,ICatalogoService catalogoService)
        {
            _carrinhoService = carrinhoService;
            _catalogoService = catalogoService;
        }

        [HttpGet("compras/carrinho")]
        public async Task<IActionResult> Index()
        {
            return CustomResponse();
        }

        [HttpGet]
        [Route("compras/carrinho-quantidade")]
        public async Task<IActionResult> ObterQuantidadeCarrinho()
        {
            return CustomResponse();
        }
        
        [HttpPost]
        [Route("compras/carrinho/item")]
        public async Task<IActionResult> AdicionarItemCarrinho()
        {
            return CustomResponse();
        }
        
        [HttpPut]
        [Route("compras/carrinho/items/{produtoId}")]
        public async Task<IActionResult> AtualizarItemCarrinho()
        {
            return CustomResponse();
        }

        [HttpDelete]
        [Route("compras/carrinho/items/{produtoId}")]
        public async Task<IActionResult> DeletarItemCarrinho()
        {
            return CustomResponse();
        }

    }
}
