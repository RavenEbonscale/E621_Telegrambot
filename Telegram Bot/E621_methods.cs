using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram_Bot;

namespace e621thing
{
    internal class E621lookeruper
    {
        //How the Program Request the things or what ever

        private static async Task<E621Classes> E621Request(string url)
        {
            API_key Api_Key = new API_key();
            var client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            client.DefaultRequestHeaders.Add("api-key", Api_Key.ApiKey);
            client.DefaultRequestHeaders.Add("user-agent", Api_Key.useragent);
            client.DefaultRequestHeaders.Add("username", Api_Key.username);
            HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);
            string response = await responseMessage.Content.ReadAsStringAsync();
            E621Classes e621 = JsonConvert.DeserializeObject<E621Classes>(response);

            return e621;
        }

        //All e621 related tasks that are calle on by the main program

        public static async Task<string> E621search(string url)
        {
            Random rnd = new Random();
            E621Classes e621 = await E621Request(url);
            List<string> urls = new List<string>();

            foreach (var item in e621.posts)
            {
                urls.Add(item.file.url);
            }
            int index = rnd.Next(urls.Count);
            return urls[index];
        }

        public static async Task<(List<string>, List<string>)> E621spam(string url)
        {
            E621Classes e621 = await E621Request(url);
            List<string> superurls = new List<string>();
            List<string> supermd5 = new List<string>();
            {
                foreach (var item in e621.posts)
                {
                    superurls.Add(item.file.url);
                    supermd5.Add(item.file.md5);
                }
                return (superurls, supermd5);
            }
        }

        public static async Task<(string, string)> E621Imageidle(string url)
        {
            E621Classes e621 = await E621Request(url);
            List<string> urls = new List<string>();
            List<string> md5 = new List<string>();

            foreach (var item in e621.posts)
            {
                urls.Add(item.file.url);
                md5.Add(item.file.md5);
            }

            return (urls[0], md5[0]);
        }
    }
}