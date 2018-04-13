using System;
using System.Linq;
using ApiTestEngine.Shared.Models;
using ApiTestEngine.Shared.Services.Interfaces;

namespace ApiTestEngine.Shared.Services
{
    public class ResponseProcessor : IResponseProcessor
    {
        private readonly IBasicLogger _logger;
        private readonly HandleBarParser _parser;

        public ResponseProcessor(HandleBarParser parser, IBasicLogger logger)
        {
            _parser = parser;
            this._logger = logger;
        }

        public void ProcessResponse(GatewayResponseMessage gatewayResponseMessage, ApiRequestModel apiRequestModel)
        {
            if (!string.IsNullOrEmpty(apiRequestModel.ResponseStoreKey))
            {
                if (apiRequestModel.StoreType.ToLower() == "global")
                {
                    _logger.LogTrace($"{apiRequestModel.CorrelationId} Upserted to Global Variables Pool");
                    HandleBarParser.UpsertGlobalVariablePool(apiRequestModel.ResponseStoreKey, gatewayResponseMessage.ResponseMessage);
                }
                else
                    _logger.LogTrace($"{apiRequestModel.CorrelationId} Upserted to Local Variables Pool");
                {
                    _parser.UpsertLocalVariable(apiRequestModel.ResponseStoreKey, gatewayResponseMessage.ResponseMessage);
                }
            }
            if (apiRequestModel.ExpectedStatusCode?.Length > 0)
            {
                _logger.LogTrace($"{apiRequestModel.CorrelationId} Asserting status ");
                if (!apiRequestModel.ExpectedStatusCode.Contains((int)gatewayResponseMessage.StatusCode))
                {
                    throw new InvalidOperationException("Expected status code not found");
                }
            }
            if (apiRequestModel.ExpectedResponse?.Length > 0)
            {
                _logger.LogTrace($"{apiRequestModel.CorrelationId} Asserting Response ");
                if (!apiRequestModel.ExpectedResponse.Contains(gatewayResponseMessage.ResponseMessage))
                {
                    throw new InvalidOperationException("Expected response not found");
                }
            }
        }
    }
}