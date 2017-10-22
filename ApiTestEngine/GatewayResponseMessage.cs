using System.Net;

namespace ApiTestEngine
{
    public class GatewayResponseMessage
    {
        public string ResponseMessage { get; internal set; }
        public HttpStatusCode StatusCode { get; internal set; }
    }
}