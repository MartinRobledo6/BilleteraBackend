using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCriptomonedas.Model
{
    public class Transacciones
    {
        [Key]
        public int Id { get; set; }
        public string CryptoCode { get; set; }
        public string Action { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal CryptoAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Money { get; set; }
        public DateTime DateTime { get; set; }
    }
}