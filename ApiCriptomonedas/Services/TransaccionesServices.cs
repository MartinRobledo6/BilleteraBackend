using ApiCriptomonedas.Models;
using ApiCriptomonedas.DTOs;
using ApiCriptomonedas.Interfaces;
using ApiCriptomonedas.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiCriptomonedas.Services
{
    public class TransaccionesServices : ITransaccioneService
    {
        private readonly AppDbContext _context;

        public TransaccionesServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transacciones>> GetTransacciones()
        {
            return await _context.Transacciones
                           .OrderByDescending(t => t.DateTime)
                           .ToListAsync();
        }

        public async Task<Transacciones> AddTransaccion(TransaccionesDTO transaccion)
        {
            var nuevaTransaccion = new Transacciones
            {
                CryptoCode = transaccion.crypto_code,
                Action = transaccion.action,
                CryptoAmount = transaccion.crypto_amount,
                Money = transaccion.money,
                DateTime = transaccion.datetime,
            };

            _context.Transacciones.Add(nuevaTransaccion);
            await _context.SaveChangesAsync();

            return nuevaTransaccion;
        }

    }
}