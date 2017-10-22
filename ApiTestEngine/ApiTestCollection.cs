using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTestEngine
{
    public class ApiTestCollection
    {
        public Dictionary<string, ApiRequestModel> ApiRequestModelsCollection { get; set; }
        public Dictionary<string,ApiRequestPlaylist> ApiPlaylistCollection { get; set; }

        public ApiRequestPlaylist GetApiRequestPlayList(string playListId)
        {
            return ApiPlaylistCollection[playListId];
        }

        public Dictionary<string, string> GlobalVariablesCollection { get; set; }

    }
}
