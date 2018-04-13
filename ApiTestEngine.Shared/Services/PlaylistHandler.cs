using ApiTestEngine.Shared.Models;
using ApiTestEngine.Shared.Services.Interfaces;
using Newtonsoft.Json;

namespace ApiTestEngine.Shared.Services
{
    /// <summary></summary>
    public class PlaylistHandler
    {
        /// <summary> The active play list identifier </summary>
        private string _activePlayListId;

        /// <summary> The API test collection </summary>
        private ApiTestCollection _apiTestCollection;

        /// <summary> The API test play list </summary>
        private ApiRequestPlaylist _apiTestPlayList;

        /// <summary> The handler </summary>
        private HttpRequestExecutionHandler _handler;

        /// <summary> The parser </summary>
        private HandleBarParser _parser;

        /// <summary> The response processor </summary>
        private IResponseProcessor _responseProcessor;
        private IBasicLogger _logger;

        /// <summary> Initializes a new instance of the <see cref="PlaylistHandler" /> class. </summary>
        /// <param name="playListId">        The play list identifier. </param>
        /// <param name="parser">            The parser. </param>
        /// <param name="handler">           The handler. </param>
        /// <param name="apiTestCollection"> The API test collection. </param>
        /// <param name="responseProcessor"> The response processor. </param>
        public PlaylistHandler(string playListId,
                                IBasicLogger logger,
                                HandleBarParser parser,
                                HttpRequestExecutionHandler handler,
                                ApiTestCollection apiTestCollection,
                                IResponseProcessor responseProcessor)
        {
            this._activePlayListId = playListId;
            this._parser = parser;
            this._handler = handler;
            this._apiTestCollection = apiTestCollection;
            this._responseProcessor = responseProcessor;
            this._apiTestPlayList = apiTestCollection.ApiPlaylistCollection[_activePlayListId];
            _parser = new HandleBarParser(_apiTestPlayList.LocalVariablesCollection);
            HandleBarParser.UpsertGlobalVariablePool(apiTestCollection.GlobalVariablesCollection);
            this._logger = logger;
        }

        /// <summary> Parses the response. </summary>
        /// <param name="gatewayResponseMessage"> The gateway response message. </param>
        /// <param name="apiRequestModel">        The API request model. </param>
        /// <returns></returns>
        public GatewayResponseMessage ParseResponse(GatewayResponseMessage gatewayResponseMessage, ApiRequestModel apiRequestModel)
        {
            _logger.LogTrace($"{apiRequestModel.CorrelationId} Run-Api-Parsing Response {JsonConvert.SerializeObject(gatewayResponseMessage)}");

            _responseProcessor.ProcessResponse(gatewayResponseMessage, apiRequestModel);
            _logger.LogTrace($"{apiRequestModel.CorrelationId} Run-Api-Initated");

            return gatewayResponseMessage;
        }

        /// <summary> Runs the API. </summary>
        /// <param name="apiTestCollection"> The API test collection. </param>
        /// <param name="apiId">             The API identifier. </param>
        /// <returns></returns>
        public GatewayResponseMessage RunApi(ApiTestCollection apiTestCollection, string apiId)
        {
            var apiRequestModel = apiTestCollection.ApiRequestModelsCollection[apiId];
            return RunApi(apiRequestModel);
        }

        /// <summary> Runs the API. </summary>
        /// <param name="apiTestCollection"> The API test collection. </param>
        /// <param name="apiRequestModel">   The API request model. </param>
        /// <returns></returns>
        public GatewayResponseMessage RunApi(ApiRequestModel apiRequestModel)
        {
            _logger.LogTrace($"{apiRequestModel.CorrelationId} Run-Api-Initated");
            apiRequestModel = ApiRequestModel.GetHandelBarReplacedRequest(apiRequestModel, _parser);
            var resp = _handler.ExecuteApiRequest(apiRequestModel);
            _logger.LogTrace($"{apiRequestModel.CorrelationId} Run-Api-Completed");

            return ParseResponse(resp, apiRequestModel);
        }



        /// <summary> Upserts the global variable. </summary>
        /// <param name="key">   The key. </param>
        /// <param name="value"> The value. </param>
        public void UpsertGlobalVariable(string key, string value)
        {
            HandleBarParser.UpsertGlobalVariablePool(key, value);
        }

        /// <summary> Upserts the local variable. </summary>
        /// <param name="key">   The key. </param>
        /// <param name="value"> The value. </param>
        public void UpsertLocalVariable(string key, string value)
        {
            _parser.UpsertLocalVariable(key, value);
        }
    }
}