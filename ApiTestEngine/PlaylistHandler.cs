

using RestSharp;
using System;

namespace ApiTestEngine.HttpHandelr
{
    public class PlaylistHandler
    {
        private string _activePlayListId;
        private HandleBarParser _parser;
        private HttpRequestExecutionHandler _handler;
        private ApiTestCollection _apiTestCollection;
        private ResponseProcessor _responseProcessor;
        private ApiRequestPlaylist _apiTestPlayList;

        public PlaylistHandler(string playListId, HandleBarParser parser,
                                HttpRequestExecutionHandler handler,
                                ApiTestCollection apiTestCollection)
        {
            this._activePlayListId = playListId;
            this._parser = parser;
            this._handler = handler;
            this._apiTestCollection = apiTestCollection;
            this._responseProcessor = new ResponseProcessor(_parser);
            this._apiTestPlayList = apiTestCollection.ApiPlaylistCollection[_activePlayListId];
            _parser = new HandleBarParser(_apiTestPlayList.LocalVariablesCollection);
            HandleBarParser.UpsertGlobalVariablePool(apiTestCollection.GlobalVariablesCollection);
        }

        public void UpsertGlobalVariable(string key, string value)
        {
            HandleBarParser.UpsertGlobalVariablePool(key, value);
        }

        public void UpsertLocalVariable(string key, string value)
        {
            _parser.UpsertLocalVariable(key, value);
        }

        //public void RunPlayList(ApiTestCollection apiTestCollection, string playListId)
        //{
        //    var apiTestPlayList = apiTestCollection.GetApiRequestPlayList(playListId);
        //    HandleBarParser.UpsertGlobalVariablePool(apiTestCollection.GlobalVariablesCollection);
        //    _parser = new HandleBarParser(apiTestPlayList.LocalVariablesCollection);

        //    foreach (var item in apiTestPlayList.ApiModelIds)
        //    {
        //        var apiRequestModel = apiTestCollection.ApiRequestModelsCollection[item];
        //        IRestResponse<object> response = _restSharp.ExecuteApiRequest<object>(apiRequestModel);
        //        _responseProcessor.ProcessResponse(response, apiRequestModel);
        //    }

        //}

        public GatewayResponseMessage RunApi(ApiTestCollection apiTestCollection, string apiId) 
        { 
            var apiRequestModel = apiTestCollection.ApiRequestModelsCollection[apiId];
            apiRequestModel = ApiRequestModel.GetHandelBarReplacedRequest(apiRequestModel, _parser);
            var resp= _handler.ExecuteApiRequest(apiRequestModel);
            return ParseResponse(resp, apiRequestModel);
        }

        public GatewayResponseMessage ParseResponse(GatewayResponseMessage gatewayResponseMessage,ApiRequestModel apiRequestModel)
        {
            _responseProcessor.ProcessResponse(gatewayResponseMessage, apiRequestModel);
            return gatewayResponseMessage;
        }


    }
}