using e621thing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace Telegram_Bot
{
    internal class Program
    {
        private static ITelegramBotClient botClient;
        
        
        private static void Main()
        {
            
     
            Apikeys api = new Apikeys();
            botClient = new TelegramBotClient(api.ApiKeytele);

            var me = botClient.GetMeAsync().Result;

            Console.WriteLine($"{botClient.GetMeAsync().Result}");
            Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");

            botClient.OnMessage += Bot_OnMessage;

            botClient.OnInlineQuery += DragonMessageAsync;

            botClient.StartReceiving();

            //Grabs the latest image from the tag a person specifies
            Images();

            //how you stop the program:P
            Thread.Sleep(-1);
            
        }
        
        private static async void DragonMessageAsync(object sender, InlineQueryEventArgs e)
        {
            Console.WriteLine(e.InlineQuery.Query);
            
            string tags = e.InlineQuery.Query.Replace(",", "+");
            string urls =  $"https://e621.net/posts.json?tags={tags}";
            (List<string>, List<string>) ImageUrl = await E621lookeruper.E621spam(urls);

                List<InlineQueryResultBase> results = new List<InlineQueryResultBase>();

                foreach (string iurl in ImageUrl.Item1.Skip(0).Take(30))
                {
                if (iurl != null)
                {
                    Console.WriteLine(iurl);
                    try
                    {
                        results.Add(new InlineQueryResultPhoto(VarFunctions.RNG(), iurl, iurl));
                    }
                    catch
                    {

                    }
                    
                }
                    
                }
               await botClient.AnswerInlineQueryAsync(e.InlineQuery.Id, results,isPersonal: true,cacheTime: 0);
                
            }
            

           
        

        //Telgregam related things

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            string check = e.Message.Type.ToString();
            string username = e.Message.From.Username;
            string Gayboi = e.Message.From.FirstName;
            Boolean blewup = true;

            Console.WriteLine(Gayboi);

            //Will stop the bot from crashing when it is sent a sticker

            if (check == "Text")
            {
                string text = e.Message.Text.ToLower();
                //List of commands the bot can do so far

                if (text.StartsWith("/settags"))

                {
                    if (Directory.Exists(@$"./users/{username}"))
                    {
                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Your tags are now:\n" + e.Message.Text.Replace("/settags", "").Trim());
                        using StreamWriter file = new StreamWriter(@$"./Users/{username}/messsage.txt");
                        file.WriteLine(e.Message.Chat.Id);
                        file.WriteLine(e.Message.Text.Replace("/settags", "").Trim().Replace(",", "+"));
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Your tags are now:\n" + e.Message.Text.Replace("/settags", "").Trim());
                        Directory.CreateDirectory(@$"./users/{username}");
                        using StreamWriter file = new StreamWriter(@$"./users/{username}/messsage.txt");
                        file.WriteLine(e.Message.Chat.Id);
                        file.WriteLine(e.Message.Text.Replace("/settags", "").Trim().Replace(",", "+"));
                    }
                }

                if (text.StartsWith("/search"))
                {
                    string message = text.Replace("/search", "").Replace(",", "+").Trim();
                    //this is incase the filesize is to large

                    while (blewup)
                        try
                        {
                            string url = $"https://e621.net/posts.json?tags={message}";
                            string imageurl = await E621lookeruper.E621search(url);
                            ;
                            await botClient.SendTextMessageAsync(e.Message.Chat, $"Grabing your image {Gayboi}");
                            await botClient.SendPhotoAsync(chatId: e.Message.Chat, await VarFunctions.ImageGetter(imageurl));

                            blewup = false;
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            await botClient.SendTextMessageAsync(e.Message.Chat, $"I'm sorry {Gayboi} {message} does not seem to be a valid tag");
                            blewup = false;
                        }
                        catch
                        {
                        }
                }

                if (text.StartsWith("/dance"))
                {
                    Console.WriteLine(username);
                    string video = @".\video_2020-11-07_00-43-43.mp4";
                    using var stream = System.IO.File.OpenRead(video);
                    await botClient.SendTextMessageAsync(e.Message.Chat, "I'll Keep on dancing as long as there is music!!");
                    await botClient.SendVideoAsync(e.Message.Chat, video: stream, supportsStreaming: true);
                }

                if (text.StartsWith("/getgroup"))
                {
                    Console.WriteLine(e.Message.From.Username);
                    string message = text.Replace("/getgroup", "").Trim().Replace(",", "+");

                    try
                    {
                        string url = $"https://e621.net/posts.json?tags={message}";

                        (List<string>, List<string>) imageurls = await E621lookeruper.E621spam(url);
                        List<InputMediaPhoto> mediaim = new List<InputMediaPhoto>();
                        List<InputMediaVideo> mediavid = new List<InputMediaVideo>();
                        await botClient.SendTextMessageAsync(e.Message.Chat, $"I'm sorry {Gayboi} this may take a while");
                        foreach (string imageurl in imageurls.Item1.Take(10))

                            try
                            {
                                if (!imageurl.Contains(".webm"))
                                {
                                    MemoryStream Is = await VarFunctions.ImageGetter(imageurl);

                                    mediaim.Add(new InputMediaPhoto(new InputMedia(Is, VarFunctions.RNG().ToString())));
                                    Console.WriteLine(imageurl);
                                }
                            }
                            catch { }

                        await botClient.SendMediaGroupAsync(mediaim, e.Message.Chat);
                    }
                    catch(Telegram.Bot.Exceptions.ApiRequestException)
                    {
                        await botClient.SendTextMessageAsync(e.Message.Chat, $"I'm sorry {Gayboi} the files are big OwO");
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(e.Message.Chat, $"I'm sorry {Gayboi} {message} doesn't to be tag");
                    }
                }

                if (e.Message.Text.StartsWith("/deletetags"))
                {
                    try
                    {
                        await botClient.SendTextMessageAsync(e.Message.Chat, $"No more tags :3", disableNotification: true);
                        System.IO.File.Delete(@$"./Users/{username}/messsage.txt");
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(e.Message.Chat, $"Sorry{Gayboi}, you have not set any tags yet~");
                    }
                }

                if (e.Message.Text.Contains("raven ebonscale"))

                {
                    await botClient.SendStickerAsync(chatId: e.Message.Chat, sticker: "CAACAgEAAxkBAAIDMF-njrB2b0k1qppbikGE7020NepdAAITAAMuHvUPrpbJaXUZk8UeBA");
                }
                
                if(e.Message.Text.Contains("help"))
                {
                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: $"Hi,{Gayboi} here are a list of command (/settags, /search, /dance, /getgroup). Seperate tags with a ',' example:(/search dragon,male,comic) ");
                }
            }
            //Dumb error message of sorts
            else
            {
                await botClient.SendStickerAsync(chatId: e.Message.Chat, sticker: "CAACAgEAAxkBAAIDPF-nkCcsrKdDafNV1JoOONY55pLjAAIbAAMuHvUPFTQxeYJHEfceBA");
            }
        }

        private static async void Images()
        {
            while (true)
            {
                if (Directory.Exists(@"./users"))
                {
                    var dics = Directory.GetDirectories(@"./users");
                    foreach (var dic in dics)
                    {
                        try
                        {
                            if (System.IO.File.Exists(@$"{dic}\messsage.txt"))
                            {
                                Console.WriteLine("step 2");
                                using StreamReader file = new StreamReader(@$"{dic}\messsage.txt");
                                string id = file.ReadLine();
                                string tags = file.ReadLine();
                                string url = $"https://e621.net/posts.json?tags={tags}";
                                var imageurl = await E621lookeruper.E621Imageidle(url);
                                var webClient = new WebClient();
                                byte[] imageBytes = webClient.DownloadData(imageurl.Item1);
                                MemoryStream ms = new MemoryStream(imageBytes);
                                await botClient.SendTextMessageAsync(chatId: id, "Here comes your image OwO");
                                await botClient.SendPhotoAsync(chatId: id, ms);
                            }
                        }
                        catch
                        { }
                    }
                }
                else { }
                Thread.Sleep(900000);
            }
        }
    }
}