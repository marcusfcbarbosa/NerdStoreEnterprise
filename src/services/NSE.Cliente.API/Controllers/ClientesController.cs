using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSE.Cliente.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.WebApi.Core.Controllers;
using System;
using System.Threading.Tasks;

namespace NSE.Cliente.API.Controllers
{
    
    public class ClientesController : MainController
    {
        private readonly IMediatrHandler _mediator;

        public ClientesController(IMediatrHandler mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("clientes")]
        public async Task<ActionResult> Index()
        {
            var result =  await _mediator.EnviarCommando(new RegistrarClientCommand(Guid.NewGuid(),"marcus",$"marcus{Guid.NewGuid().ToString()}@teste.com","21015511872"));
            return CustomResponse(result);
        }
    }
}