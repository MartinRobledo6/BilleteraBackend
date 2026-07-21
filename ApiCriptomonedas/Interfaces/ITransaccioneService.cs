using ApiCriptomonedas.DTOs;
using ApiCriptomonedas.Model;

namespace ApiCriptomonedas.Interfaces
{
    public interface ITransaccioneService
    {
        Task<List<TransaccionesDTO>> Get();
        Task<TransaccionesDTO?> GetTransacciones(int id);
        Task<TransaccionesDTO> Post(TransaccionesDTO transaccion);
        Task<bool> Patch(int id, TransaccionesDTO transacciones);
        Task<bool> DeleteTransaccion(int id);
    }
}