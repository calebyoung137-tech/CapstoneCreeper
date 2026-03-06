using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CapstoneCreeper
{
    public class PlayState
    {
        [JsonPropertyName("action_id")]
        public int ActionID { get; set; }
        [JsonPropertyName("state")]
        public string State {  get; set; }
        [JsonPropertyName("history")]
        public List<string> History { get; set; }
    }
}
