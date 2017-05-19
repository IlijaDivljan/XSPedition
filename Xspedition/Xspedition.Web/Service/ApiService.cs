using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Xspedition.Common.Commands;
using Xspedition.Web.ViewModels;

namespace Xspedition.Web.Service
{
    public sealed class XSPEditionURL
    {
        public static readonly string SCRUB = "api/events/scrubbing";
        public static readonly string NOTIFICATIONS = "api/events/notifications";
        public static readonly string RESPONSES = "api/events/responses";
        public static readonly string INSTRUCTIONS = "api/events/instructions";
        public static readonly string PAYMENTS = "api/events/payments";
    }
    public class ApiService
    {
        public CaProcessViewModel Execute(Command command)
        {
            CaProcessViewModel result = null;

            switch (command.Type)
            {
                case CommandType.Scrub:
                    ExecuteCommand(command, XSPEditionURL.SCRUB);
                    result = HandleEvent(command.CaId, XSPEditionURL.SCRUB);
                    break;
                case CommandType.Notify:
                    ExecuteCommand(command, XSPEditionURL.NOTIFICATIONS);
                    result = HandleEvent(command.CaId, XSPEditionURL.NOTIFICATIONS);
                    break;
                case CommandType.Respond:
                    ExecuteCommand(command, XSPEditionURL.RESPONSES);
                    result = HandleEvent(command.CaId, XSPEditionURL.RESPONSES);
                    break;
                case CommandType.Instruct:
                    ExecuteCommand(command, XSPEditionURL.INSTRUCTIONS);
                    result = HandleEvent(command.CaId, XSPEditionURL.INSTRUCTIONS);
                    break;
                case CommandType.Pay:
                    ExecuteCommand(command, XSPEditionURL.PAYMENTS);
                    result = HandleEvent(command.CaId, XSPEditionURL.PAYMENTS);
                    break;
            }

            return result;
        }

        public void ExecuteCommand(Command command, string url)
        {
            HttpClient httpClient = ApiHttpClient.GetHttpClient();

            string serializedEvent = JsonConvert.SerializeObject(command);
            StringContent httpContent = new StringContent(serializedEvent, Encoding.Unicode, "application/json");

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;
        }

        private CaProcessViewModel HandleEvent(int caId, string url)
        {
            HttpClient httpClient = ApiHttpClient.GetHttpClient();

            HttpResponseMessage response = httpClient.GetAsync($"{url}/{caId}").Result;
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string responseContent = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<CaProcessViewModel>(responseContent);
        }
    }
}