
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiTestEngine
{
    public class HttpClientHelper
    {
    
        /// </inheritdoc>
        public async Task<GatewayResponseMessage> SendAsync(
            string destinationUri,
            HttpMethod httpMethod,
            IDictionary<string, string> headers,
            string httpBody,
            string contentType = null)
        {
            HttpResponseMessage response = null;
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(httpMethod, destinationUri);
            SetHeader(headers, request);
            SetBody(httpBody, contentType, request);
            response = await httpClient.SendAsync(request);
            return await GetHttpResponseAsync(response);
        }
      

        private static void SetBody(string httpBody, string contentType, HttpRequestMessage request)
        {
            if (!string.IsNullOrEmpty(httpBody))
                request.Content = new StringContent(httpBody, Encoding.UTF8, contentType);
        }

        private static void SetHeader(IDictionary<string, string> headers, HttpRequestMessage request)
        {
            if (headers != null)
            {
                foreach (var headerKey in headers.Keys)
                {
                    string value;
                    if (headers.TryGetValue(headerKey, out value))
                        request.Headers.Add(headerKey, value);
                }
            }
        }


        /// <summary>
        /// Common place to process response generated via http response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal async Task<GatewayResponseMessage> GetHttpResponseAsync(HttpResponseMessage response)
        {


            GatewayResponseMessage msg = new GatewayResponseMessage()
            {
                ResponseMessage = await response.Content.ReadAsStringAsync(),
                StatusCode = response.StatusCode
            };
            return msg;
            
        }

       
    }
}
