using System.Collections.Generic;

namespace ApiTestEngine.Shared.Models
{
    /// <summary></summary>
    public class ApiTestCollection
    {
        /// <summary> Gets or sets the API playlist collection. </summary>
        /// <value> The API playlist collection. </value>
        public Dictionary<string, ApiRequestPlaylist> ApiPlaylistCollection { get; set; }

        /// <summary> Gets or sets the API request models collection. </summary>
        /// <value> The API request models collection. </value>
        public Dictionary<string, ApiRequestModel> ApiRequestModelsCollection { get; set; }

        /// <summary> Gets or sets the global variables collection. </summary>
        /// <value> The global variables collection. </value>
        public Dictionary<string, string> GlobalVariablesCollection { get; set; }

        /// <summary> Gets the API request play list. </summary>
        /// <param name="playListId"> The play list identifier. </param>
        /// <returns></returns>
        public ApiRequestPlaylist GetApiRequestPlayList(string playListId)
        {
            return ApiPlaylistCollection[playListId];
        }
    }
}