namespace Coreflow.Web.Models
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public Response() { }

        public Response(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
