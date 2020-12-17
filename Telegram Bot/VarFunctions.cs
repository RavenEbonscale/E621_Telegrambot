using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Telegram_Bot
{
    internal class VarFunctions
    {
        public static MemoryStream ImageGetter(string url)

        {
            using WebClient webClient = new WebClient();
            byte[] imageBytes = webClient.DownloadData(url);
            MemoryStream ms = new MemoryStream(imageBytes);
            return ms;
        }

        public static string RNG()
        {
            Random rng = new Random();

            return rng.Next(1, 100000000).ToString();
        }
    }
}