using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ApiTestEngine.Shared.Models;

namespace ApiTestEngine.Shared.Services.Interfaces
{
    public interface IHttpClientHelper
    {
        Task<GatewayResponseMessage> SendAsync(string destinationUri, HttpMethod httpMethod, IDictionary<string, string> headers, string httpBody, string contentType = null);
    }
}