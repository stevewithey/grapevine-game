using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Grapevine.GameRunner.Models;
using Newtonsoft.Json;

namespace Grapevine.GameRunner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var recipientUrlList = new List<string> {"https://test.com/whisper"};

            var recipientList = BuildRecipientList(recipientUrlList);

            var grapevineRequest = new MessageRequest()
            {
                GameId = 1,
                Message = "This is a test message, have fun with it!",
                NextWhisperRecipientId = 2,
                SentFromId = -1,
                WhisperRecipients = recipientList
            };

            InitiateGrapevineGame(grapevineRequest);
        }

        private static async Task InitiateGrapevineGame(MessageRequest grapevineRequest)
        {
            HttpClient httpClient = new HttpClient();

            string firstRecipientUrl = grapevineRequest.WhisperRecipients.First().Url;
            var content = new StringContent(JsonConvert.SerializeObject(grapevineRequest), Encoding.UTF8, "application/json");

            try
            {
                var result = await httpClient.PostAsync(firstRecipientUrl, content);

                if (result.IsSuccessStatusCode)
                {
                    return;
                }

                Console.WriteLine(
                    $"Failed to initiate game id {grapevineRequest.GameId}.  Response: {result.StatusCode}");
            }
            catch (SocketException socketException)
            {
                Console.WriteLine(
                    $"Failed to initiate game id {grapevineRequest.GameId}.  Unable to connect to recipient at {firstRecipientUrl}");
            }
        }

        private static List<WhisperRecipient> BuildRecipientList(List<string> recipientUrlList)
        {
            var recipientList = new List<WhisperRecipient>();

            recipientUrlList = recipientUrlList.OrderBy(i => Guid.NewGuid()).ToList();

            for (int id=0; id < recipientUrlList.Count; id++)
            {
                recipientList.Add(new WhisperRecipient()
                {
                    Id = id,
                    Url = recipientUrlList[id]
                });
            }

            return recipientList;
        }
    }
}
