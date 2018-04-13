using ApiTestEngine.Shared.Models;
using ApiTestEngine.Shared.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;


namespace ApiTestEngine.Shared.Services
{
    public class UnitTestResponseProcessor : IResponseProcessor
    {
        private readonly HandleBarParser _parser;
        private readonly IBasicLogger _logger;

        public UnitTestResponseProcessor(HandleBarParser parser, IBasicLogger logger)
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
                Assert.IsTrue(apiRequestModel.ExpectedStatusCode.Contains((int)gatewayResponseMessage.StatusCode));

            }
            if (apiRequestModel.ExpectedResponse?.Length > 0)
            {
                _logger.LogTrace($"{apiRequestModel.CorrelationId} Asserting Response ");

                Assert.IsTrue(apiRequestModel.ExpectedResponse.Contains(gatewayResponseMessage.ResponseMessage));

            }
        }
    }
}
