using ApiCriptomonedas.DTOs;
using ApiCriptomonedas.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiCriptomonedas.Controllers
{
    [ApiController]
    [Route("transacciones")]
    public class TransaccionesController : ControllerBase
    {
        private readonly ITransaccioneService _transaccioneService;

        public TransaccionesController(ITransaccioneService transaccioneService)
        {
            _transaccioneService = transaccioneService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var transacciones = await _transaccioneService.GetTransacciones();
            return Ok(transacciones);
        }

        [HttpPost]
        public async Task<IActionResult> Post(TransaccionesDTO transaccionesDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nuevaTransaccion = await _transaccioneService.AddTransaccion(transaccionesDTO);

            return Ok(nuevaTransaccion);
        }

    }
}
