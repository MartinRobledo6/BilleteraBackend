using ApiCriptomonedas.DTOs;
using ApiCriptomonedas.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiCriptomonedas.Controllers
{
    [Route("transacciones")]
    [ApiController]
    public class TransaccionesController : ControllerBase
    {
        private readonly ITransaccioneService _transaccioneService;

        public TransaccionesController(ITransaccioneService transaccioneService)
        {
            _transaccioneService = transaccioneService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransaccionesDTO>>> Get()
        {
            var transacciones = await _transaccioneService.Get();
            return Ok(transacciones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransaccionesDTO>> GetTransaccion(int id)
        {
            var transaccion = await _transaccioneService.GetTransacciones(id);
            if (transaccion == null)
            {
                return NotFound();
            }
            return Ok(transaccion);
        }

        [HttpPost]
        public async Task<ActionResult<TransaccionesDTO>> Post(TransaccionesDTO transaccionesDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var nuevaTransaccion = await _transaccioneService.Post(transaccionesDTO);
                return CreatedAtAction(nameof(GetTransaccion), new { id = nuevaTransaccion.Id }, nuevaTransaccion);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, TransaccionesDTO transaccionesDTO)
        {
            bool result = await _transaccioneService.Patch(id, transaccionesDTO);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaccion(int id)
        {
            bool result = await _transaccioneService.DeleteTransaccion(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
