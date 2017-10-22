using ApiTestEngine.HttpHandelr;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTestEngine
{
    class Program
    {

        static void Main(string[] args)
        {
            var data = File.ReadAllText("apiTestcollection.json");
            var apiTestCollection = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiTestCollection>(data);
            var apiPlaylist = apiTestCollection.ApiPlaylistCollection["play1"];
            HandleBarParser handleBarParser = new HandleBarParser(apiPlaylist.LocalVariablesCollection);
            HttpRequestExecutionHandler httpRequestExecutionHandler = new HttpRequestExecutionHandler();
            PlaylistHandler playlistHandler = new PlaylistHandler("play1", handleBarParser, httpRequestExecutionHandler, apiTestCollection);

            var response = playlistHandler.RunApi(apiTestCollection, "identity");
            var token = Newtonsoft.Json.JsonConvert.DeserializeObject<TokeResponse>(response.ResponseMessage);
            playlistHandler.UpsertGlobalVariable("access_token", token.access_token);
            response = playlistHandler.RunApi(apiTestCollection, "sas-token-post");

            //ApiRequestModel apiRequestModel = new ApiRequestModel()
            //{
            //    ApiId = "sas-token-post",
            //    ContentType = "application/json",
            //    Headers = new Dictionary<string, string>() { { "tenantId", "{{tenantId}}" } },
            //    Host = "{{active-route}}",
            //    HttpMethod = "POST",
            //    PayLoadBody = "{\r\n  \"id\": \"740cbcb4-1243-2ec6-c771-7e96e3c88f4f\",\r\n  \"tenantId\": \"00000000-0000-0000-0000-000000000000\",\r\n  \"IsEnabled\": true,\r\n  \"keys\": [\r\n    {\r\n      \"Key\": \"de42f476-7189-0de3-7ebf-ecf426c1746e\",\r\n      \"Value\": \"Endpoint=sb://sbusegeostage.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=dxP3dIfbytGUVQwG/X9zfaLzzBJYWhf5RgZwctNBFjc=\",\r\n      \"Permission\": 3\r\n    }\r\n  ]\r\n}",
            //        AssertScript = "none",
            //    ExpectedResponse = "OK",
            //    ExpectedStatusCode = new int[] { 201, 409 },
            //    ResponseStoreKey = "sas-token-post",
            //    URI = "/api/v1/gateway/sas-tokens/",
            //    StoreType ="local"


            //};
            //ApiRequestPlaylist apiRequestPlaylist = new ApiRequestPlaylist()
            //{
            //    ApiModelIds = new List<string>(),
            //    Description = "dummy",
            //    LocalVariablesCollection = new Dictionary<string, string>() { { "{{tenantId}}", "04b93955-891c-4725-b976-4325c6a53af7" } },
            //    PlayListId = "play1"
            //};

            //ApiTestCollection apiTestCollection = new ApiTestCollection()
            //{
            //    ApiPlaylistCollection = new Dictionary<string, ApiRequestPlaylist>() { { apiRequestPlaylist.PlayListId, apiRequestPlaylist } },
            //    ApiRequestModelsCollection = new Dictionary<string, ApiRequestModel>() { { apiRequestModel.ApiId, apiRequestModel } },
            //    GlobalVariablesCollection = new Dictionary<string, string>() { }
            //};

            //var x = Newtonsoft.Json.JsonConvert.SerializeObject(apiTestCollection);
            //Console.WriteLine(x);
            //File.WriteAllText("apiTestcollection1.json", x);

        }
    }
}


/*
 *   ApiRequestModel apiRequestModel = new ApiRequestModel()
            {
                ApiId = "sas-token-post",
                ContentType = "application/json",
                Headers = new Dictionary<string, string>() { { "tenantId", "{{tenantId}}" } },
                Host = "{{active-route}}",
                HttpMethod = "POST",
                PayLoadBody = "{\r\n  \"id\": \"740cbcb4-1243-2ec6-c771-7e96e3c88f4f\",\r\n  \"tenantId\": \"00000000-0000-0000-0000-000000000000\",\r\n  \"IsEnabled\": true,\r\n  \"keys\": [\r\n    {\r\n      \"Key\": \"de42f476-7189-0de3-7ebf-ecf426c1746e\",\r\n      \"Value\": \"Endpoint=sb://sbusegeostage.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=dxP3dIfbytGUVQwG/X9zfaLzzBJYWhf5RgZwctNBFjc=\",\r\n      \"Permission\": 3\r\n    }\r\n  ]\r\n}",
                ResponseProcessorMetaData = new ResponseProcessorMetaData()
                {
                    AssertScript = "none",
                    ExpectedResponse = "OK",
                    ExpectedStatusCode = new  int []{ 201, 409 },
                    ResponseStoreKey = "sas-token-post"

                },
                URI= "/api/v1/gateway/sas-tokens/"


            };
            ApiRequestPlaylist apiRequestPlaylist = new ApiRequestPlaylist()
            {
                ApiModelIds = new List<string>(),
                Description ="dummy",
                LocalVariablesCollection= new Dictionary<string, string>() { {"{{tenantId}}", "04b93955-891c-4725-b976-4325c6a53af7" } },
                PlayListId="play1"
            };

            ApiTestCollection apiTestCollection = new ApiTestCollection()
            {
                ApiPlaylistCollection=new Dictionary<string, ApiRequestPlaylist>() { {apiRequestPlaylist.PlayListId,apiRequestPlaylist } },
                ApiRequestModelsCollection=new Dictionary<string, ApiRequestModel>() { {apiRequestModel.ApiId,apiRequestModel } },
                GlobalVariablesCollection=new Dictionary<string, string>() { }
            };

            var x= Newtonsoft.Json.JsonConvert.SerializeObject(apiTestCollection);
            Console.WriteLine(x);
            File.WriteAllText("apiTestcollection.json",x);
 */
