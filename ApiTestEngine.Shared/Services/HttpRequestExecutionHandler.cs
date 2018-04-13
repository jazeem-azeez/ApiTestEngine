using ApiTestEngine.Shared.Models;
using ApiTestEngine.Shared.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;

namespace ApiTestEngine.Shared.Services
{
    public class HttpRequestExecutionHandler
    {
        private readonly IBasicLogger _logger;

        public HttpRequestExecutionHandler(IBasicLogger logger)
        {
            this._logger = logger;
        }
        public GatewayResponseMessage ExecuteApiRequest(ApiRequestModel apiRequest)
        {
            var stopwatch = Stopwatch.StartNew();
            HttpClientHelper httpClientHelper = new HttpClientHelper();
            var response = httpClientHelper.SendAsync(apiRequest.FULLURI, new HttpMethod(apiRequest.HttpMethod), apiRequest.Headers, apiRequest.PayLoadBody, apiRequest.ContentType).ConfigureAwait(false);
            var temp = response.GetAwaiter().GetResult();
            stopwatch.Stop();
            _logger.LogTrace($"Time Elapsed for {apiRequest.ApiId}:{apiRequest.CorrelationId} is {stopwatch.ElapsedMilliseconds} ms : {stopwatch.ElapsedTicks} ticks");
            return temp;
        }
    }
}
