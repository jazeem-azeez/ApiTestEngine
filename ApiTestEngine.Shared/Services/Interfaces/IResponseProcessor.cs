using ApiTestEngine.Shared.Models;

namespace ApiTestEngine.Shared.Services.Interfaces
{
    public interface IResponseProcessor
    {
        void ProcessResponse(GatewayResponseMessage gatewayResponseMessage, ApiRequestModel apiRequestModel);
    }
}