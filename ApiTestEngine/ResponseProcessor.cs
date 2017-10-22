using System;
using System.Linq;

namespace ApiTestEngine.HttpHandelr
{
    public class ResponseProcessor
    {
        private HandleBarParser _parser;

        public ResponseProcessor(HandleBarParser parser)
        {
            _parser = parser;
        }

        internal void ProcessResponse(GatewayResponseMessage gatewayResponseMessage, ApiRequestModel apiRequestModel)
        {
            if (!string.IsNullOrEmpty(apiRequestModel.ResponseStoreKey))
            {
                if (apiRequestModel.StoreType.ToLower() == "global")
                {
                    HandleBarParser.UpsertGlobalVariablePool(apiRequestModel.ResponseStoreKey, gatewayResponseMessage.ResponseMessage);
                }
                else
                {
                    _parser.UpsertLocalVariable(apiRequestModel.ResponseStoreKey, gatewayResponseMessage.ResponseMessage);
                }
            }
            if (apiRequestModel.ExpectedStatusCode?.Length > 0)
            {
                if (!apiRequestModel.ExpectedStatusCode.Contains((int)gatewayResponseMessage.StatusCode))
                {
                    throw new Exception("Expected Response Not Found");
                }
            }
            if ( apiRequestModel.ExpectedResponse?.Length>0)
            {
                if (!apiRequestModel.ExpectedResponse.Contains(gatewayResponseMessage.ResponseMessage))
                {
                    throw new Exception("UnExpected Response Message");
                }
            }
        }
    }
}