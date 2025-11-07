namespace BE_SWP391.Models.DTOs.Response
{
    public class PaymentCreateResponse
    {
        public string PaymentUrl { get; set; }
        public string TransactionRef { get; set; } // transaction id
    }
}
