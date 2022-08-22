using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSE.Carrinho.API.Model;
using NSE.WebApi.Core.Controllers;
using NSE.WebApi.Core.Usuarios;
using System;
using System.Threading.Tasks;

namespace NSE.Carrinho.API.Controllers
{
    public class CarrinhoController : MainController
    {
        private readonly IAspNetUser _user;
        public CarrinhoController(IAspNetUser user)
        {
            _user = user;
        }

        [HttpGet("carrinho")]
        public async Task<CarrinhoCliente> ObterCarrinho()
        {
            return  new CarrinhoCliente(System.Guid.Empty);
        }

        [HttpPost("carrinho")]
        public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem item)
        {

            return CustomResponse();
        }
        [HttpPut("carrinho/{produtoId}")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, CarrinhoItem item)
        {
            
            return CustomResponse();
        }

        [HttpDelete("carrinho/{produtoId}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            
            return CustomResponse();
        }
    }
}
