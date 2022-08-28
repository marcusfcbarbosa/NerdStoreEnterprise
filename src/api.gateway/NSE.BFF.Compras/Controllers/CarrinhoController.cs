using Microsoft.AspNetCore.Mvc;
using NSE.WebApi.Core.Controllers;
using System.Threading.Tasks;

namespace NSE.BFF.Compras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrinhoController : MainController
    {
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
