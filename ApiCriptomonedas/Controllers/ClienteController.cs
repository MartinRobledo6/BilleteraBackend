using ApiCriptomonedas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace ApiCriptomonedas.Controllers
{
    [ApiController]
    [Route("clientes")]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> Get()
        {
            var clientes = await _context.Cliente.ToListAsync();
            return Ok(clientes);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Post(Cliente nuevoCliente)
        {
            if(string.IsNullOrEmpty(nuevoCliente.Name) || string.IsNullOrEmpty(nuevoCliente.Email))
            {
                return BadRequest("El nombre y el correo electrónico son obligatorios.");
            }

            _context.Cliente.Add(nuevoCliente);
            await _context.SaveChangesAsync();
            return Ok(nuevoCliente);
        }
    }
}
