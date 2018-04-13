using System.IO;
using System.Linq;
using ApiTestEngine.Shared.Models;
using ApiTestEngine.Shared.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace SampleUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        private HttpRequestExecutionHandler _httpRequestExecutionHandler;
        private PlaylistHandler _playlistHandler;
        public ApiRequestPlaylist _apiPlaylist { get; private set; }
        public ApiTestCollection _apiTestCollection { get; private set; }
        public HandleBarParser _handleBarParser { get; private set; }
        internal FileLogger _logger { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            var _playListdata = File.ReadAllText(@"C:\SourceRepo\ApiTestEngine\SampleUnitTestProject\SamplePlayList.json");
            _logger = new FileLogger();
            _apiTestCollection = JsonConvert.DeserializeObject<ApiTestCollection>(_playListdata);
            _apiPlaylist = _apiTestCollection.ApiPlaylistCollection["play1"];
            _handleBarParser = new HandleBarParser(_apiPlaylist.LocalVariablesCollection);
            _httpRequestExecutionHandler = new HttpRequestExecutionHandler(_logger);
            _playlistHandler = new PlaylistHandler
                                ("play1",
                                _logger,
                                _handleBarParser,
                                _httpRequestExecutionHandler,
                                _apiTestCollection,
                                new UnitTestResponseProcessor(_handleBarParser, _logger)
                                );
            var response = _playlistHandler.RunApi(_apiTestCollection, "mockget"); 
        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(HandleBarParser.GlobalVariablePool.ContainsKey("{{mockget_hello}}"));
        }
    }
}