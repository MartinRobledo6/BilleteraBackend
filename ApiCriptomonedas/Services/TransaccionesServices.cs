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

        public async Task<List<TransaccionesDTO>> Get()
        {
            var transacciones = await _context.Transacciones
                                       .OrderByDescending(t => t.DateTime)
                                       .ToListAsync();
            var transaccionesDTO = transacciones.Select(t => new TransaccionesDTO
            {
                Id = t.Id,
                crypto_code = t.CryptoCode,
                action = t.Action,
                crypto_amount = t.CryptoAmount,
                money = t.Money,
                datetime = t.DateTime,
            }).ToList();

            return transaccionesDTO;
        }

        public async Task<TransaccionesDTO?> GetTransacciones(int id)
        {
            var transacciones = await _context.Transacciones.FirstOrDefaultAsync(g => g.Id == id);
            if (transacciones == null)
            {
                return null;
            }
            return new TransaccionesDTO
            {
                Id = transacciones.Id,
                crypto_code = transacciones.CryptoCode,
                action = transacciones.Action,
                crypto_amount = transacciones.CryptoAmount,
                money = transacciones.Money,
                datetime = transacciones.DateTime,
            };
        }

        public async Task<TransaccionesDTO> Post(TransaccionesDTO transaccion)
        {
            string cryptoCodeLower = transaccion.crypto_code.ToLower();

            string urlCriptoya = $"https://criptoya.com/api/fiwind/{cryptoCodeLower}/ars";

            decimal precio = 0;

            try
            {
                var respuesta = await _httpClient.GetAsync(urlCriptoya);

                if (respuesta.IsSuccessStatusCode)
                {
                    var jsonString = await respuesta.Content.ReadAsStringAsync();
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
                throw new Exception("Error al consultar CryptoYa: " + ex.Message);
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
            return transaccion;
        }

        public async Task<bool> Put(int id, TransaccionesDTO transaccionDTO)
        {
            if (id != transaccionDTO.Id)
            {
                return false;
            }
            var transaccionExistente = await _context.Transacciones.FindAsync(id);
            if (transaccionExistente == null)
            {
                return false;
            }

            transaccionExistente.CryptoCode = transaccionDTO.crypto_code;
            transaccionExistente.Action = transaccionDTO.action;
            transaccionExistente.CryptoAmount = transaccionDTO.crypto_amount;
            transaccionExistente.Money = transaccionDTO.money;
            transaccionExistente.DateTime = transaccionDTO.datetime;

            _context.Entry(transaccionExistente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTransaccion(int id)
        {
            var transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion == null)
            {
                return false;
            }

            _context.Transacciones.Remove(transaccion);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}