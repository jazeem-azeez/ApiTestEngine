using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTestEngine
{
    public class ApiRequestPlaylist
    {
        public string PlayListId { get; set; }
        public string Description { get; set; }
        public List<string> ApiModelIds { get; set; }
        public Dictionary<string, string> LocalVariablesCollection { get; set; }

    }
}
