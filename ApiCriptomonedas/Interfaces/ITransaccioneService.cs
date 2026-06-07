using ApiCriptomonedas.DTOs;
using ApiCriptomonedas.Model;
using ApiCriptomonedas.Models;

namespace ApiCriptomonedas.Interfaces
{
    public interface ITransaccioneService
    {
        Task<IEnumerable<Transacciones>> GetTransacciones();
        Task<Transacciones> AddTransaccion(TransaccionesDTO transaccion);
    }
}