namespace ApiCriptomonedas.DTOs
{
    public class TransaccionesDTO
    {
        public string crypto_code { get; set; }
        public string action { get; set; }
        public decimal crypto_amount { get; set; }
        public decimal money { get; set; }
        public DateTime datetime { get; set; }
    }
}
