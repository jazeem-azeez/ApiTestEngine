using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiTestEngine
{
    public class HttpRequestExecutionHandler
    {
        public GatewayResponseMessage ExecuteApiRequest(ApiRequestModel apiRequest) 
        {
           // var response = string.Empty;
            HttpClientHelper httpClientHelper = new HttpClientHelper();
            var response = httpClientHelper.SendAsync(apiRequest.FULLURI, new HttpMethod(apiRequest.HttpMethod), apiRequest.Headers, apiRequest.PayLoadBody, apiRequest.ContentType).ConfigureAwait(false);
            return response.GetAwaiter().GetResult();

        }
    }
}
