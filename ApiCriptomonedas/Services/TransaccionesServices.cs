using ApiCriptomonedas.Models;
using ApiCriptomonedas.DTOs;
using ApiCriptomonedas.Interfaces;
using ApiCriptomonedas.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ApiCriptomonedas.Services
{
    public class TransaccionesServices : ITransaccioneService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public TransaccionesServices(AppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Transacciones>> GetTransacciones()
        {
            return await _context.Transacciones
                           .OrderByDescending(t => t.DateTime)
                           .ToListAsync();
        }

        public async Task<Transacciones> AddTransaccion(TransaccionesDTO transaccion)
        {
            string cryptoCodeLower = transaccion.crypto_code.ToLower();

            string urlCriptoya = $"https://criptoya.com/api/fiwind/{cryptoCodeLower}/ars";

            decimal precio = 0;

            try
            {
                var response = await _httpClient.GetAsync(urlCriptoya);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(jsonString))
                    {
                        precio = doc.RootElement.GetProperty("totalAsk").GetDecimal();
                    }
                }
                else
                {
                    throw new Exception("No se pudo obtener el precio desde CryptoYa");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar CryptoYa" + ex.Message);
            }

            decimal totalGastado = precio * transaccion.crypto_amount;

            var nuevaTransaccion = new Transacciones
            {
                CryptoCode = transaccion.crypto_code,
                Action = transaccion.action,
                CryptoAmount = transaccion.crypto_amount,
                Money = totalGastado,
                DateTime = transaccion.datetime,
            };

            _context.Transacciones.Add(nuevaTransaccion);
            await _context.SaveChangesAsync();

            return nuevaTransaccion;
        }

    }
}