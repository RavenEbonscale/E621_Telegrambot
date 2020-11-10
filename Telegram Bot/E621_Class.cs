using Newtonsoft.Json;
using System.Collections.Generic;

namespace Telegram_Bot
{
    internal class API_key
    {

        public List<string> urls = new List<string>();
    }

    public class E621Classes
    {
        public Post[] posts { get; set; }
    }

    public class Post
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("created_at")]
        public string created_at { get; set; }

        [JsonProperty("file")]
        public File file { get; set; }
    }

    public class File

    {
        [JsonProperty("url")]
        public string url { get; set; }

        [JsonProperty("md5")]
        public string md5 { get; set; }
    }
}