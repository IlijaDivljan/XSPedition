using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xspedition.Common.Commands;

namespace Xspedition.Loader
{
    public class DataLoader
    {
        public static void SendScrubCommand(Command command)
        {
            ScrubCaCommand scrubCommand = (ScrubCaCommand)command;

            POST(JsonConvert.SerializeObject(scrubCommand), "SendScrubbingInputData");
        }

        public static void SendNotifyCommand(Command command)
        {
            NotifyCommand notifyCommand = (NotifyCommand)command;

            POST(JsonConvert.SerializeObject(notifyCommand), "SendNotificationInputData");
        }

        public static void SendResponsCommand(Command command)
        {
            RespondCommand responsCommand = (RespondCommand)command;

            POST(JsonConvert.SerializeObject(responsCommand), "SendResponseInputData");
        }

        public static void SendInstructCommand(Command command)
        {
            InstructCommand instructCommand = (InstructCommand)command;

            POST(JsonConvert.SerializeObject(instructCommand), "SendInstructionInputData");
        }

        public static void SendPayCommand(Command command)
        {
            PayCommand payCommand = (PayCommand)command;

            POST(JsonConvert.SerializeObject(payCommand), "SendPaymentInputData");
        }



        public static void POST(string serializedCommand, string target)
        {
            string apiUrl = @"http://xspedition.azurewebsites.net/xspedition/" + target;
            string proxyUri = string.Format("{0}:{1}", "http://10.244.176.15", 8080);

            NetworkCredential proxyCreds = new NetworkCredential(
                "ilija.divljan",
                "Pa5word!"
            );

            WebProxy proxy = new WebProxy(proxyUri, false)
            {
                UseDefaultCredentials = false,
                Credentials = proxyCreds,
            };
            
            HttpClient client = null;
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
                PreAuthenticate = true,
                UseDefaultCredentials = false,
            };

            client = new HttpClient(httpClientHandler);
            var response = client.PostAsync(apiUrl, new StringContent(serializedCommand, Encoding.UTF8, "application/json")).Result;
        }
    }
}
