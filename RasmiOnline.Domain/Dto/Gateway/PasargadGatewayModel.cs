namespace RasmiOnline.Domain.Dto
{
    public class PasargadGetTokenReponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public string Token { get; set; }
    }

    public class PasargadVerifyResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int TraceNumber { get; set; }
        public string TransactionDate { get; set; }
        public string Action { get; set; }
        public string TransactionReferenceID { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public int MerchantCode { get; set; }
        public int TerminalCode { get; set; }
        public decimal Amount { get; set; }
    }
}
