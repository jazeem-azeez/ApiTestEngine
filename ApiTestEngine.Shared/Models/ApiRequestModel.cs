using ApiTestEngine.Shared.Services;
using System;
using System.Collections.Generic;

namespace ApiTestEngine.Shared.Models
{
    public class ApiRequestModel
    {
        public string CorrelationId { get; set; }
        public string ApiId { get; set; }
        public string Host { get; set; }
        public string HttpMethod { get; set; }
        public string URI { get; set; }
        public string FULLURI { get { return Host.TrimEnd('/') + "/" + URI.TrimStart('/'); } }
        public Dictionary<string, string> Headers { get; set; }
        public string PayLoadBody { get; set; }
        public string ResponseStoreKey { get; set; }
        public string StoreType { get; set; }
        public string[] ExpectedResponse { get; set; }
        public int[] ExpectedStatusCode { get; set; }
        public string AssertScript { get; set; }
        public string ContentType { get; set; }

        public static ApiRequestModel GetHandelBarReplacedRequest(ApiRequestModel apiRequestModel, HandleBarParser handleBarParser)
        {
            ApiRequestModel result = new ApiRequestModel()
            {
                ApiId = handleBarParser.ProcessInputString(apiRequestModel.ApiId),
                Host = handleBarParser.ProcessInputString(apiRequestModel.Host),
                HttpMethod = handleBarParser.ProcessInputString(apiRequestModel.HttpMethod),
                URI = handleBarParser.ProcessInputString(apiRequestModel.URI),
                PayLoadBody = handleBarParser.ProcessInputString(apiRequestModel.PayLoadBody),
                ContentType = handleBarParser.ProcessInputString(apiRequestModel.ContentType),
                AssertScript = handleBarParser.ProcessInputString(apiRequestModel.AssertScript),
                StoreType = handleBarParser.ProcessInputString(apiRequestModel.StoreType),
                ResponseStoreKey = handleBarParser.ProcessInputString(apiRequestModel.ResponseStoreKey),
                ExpectedResponse = handleBarParser.ProcessInputString(apiRequestModel.ExpectedResponse),
                ExpectedStatusCode = apiRequestModel.ExpectedStatusCode,
                Headers = handleBarParser.ProcessInputString(apiRequestModel.Headers),
                CorrelationId = string.IsNullOrEmpty(apiRequestModel.CorrelationId) ? $"{Guid.NewGuid().ToString()}-{apiRequestModel.ApiId}" : apiRequestModel.CorrelationId
            };

            return result;
        }
    }
}
