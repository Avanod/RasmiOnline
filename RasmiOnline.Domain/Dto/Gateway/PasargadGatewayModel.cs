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


    }
}
