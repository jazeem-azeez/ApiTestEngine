using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApiTestEngine.Shared.Models
{
    public class GatewayResponseMessage
    {
        public string ResponseMessage { get; internal set; }
        public HttpStatusCode StatusCode { get; internal set; }
    }
}
