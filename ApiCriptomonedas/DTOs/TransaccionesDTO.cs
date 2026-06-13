using System.ComponentModel.DataAnnotations;

namespace ApiCriptomonedas.DTOs
{
    public class TransaccionesDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La criptomoneda es obligatoria")]
        public string crypto_code { get; set; }

        [Required(ErrorMessage = "La acción es obligatoria")]
        public string action { get; set; }

        [Range(0.00000001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public decimal crypto_amount{ get; set; }
        public decimal money { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime datetime { get; set; }
        public int ClienteID { get; set; }

    }
}
