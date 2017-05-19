using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Xspedition.Common.Commands;
using Xspedition.Web.Hub;
using Xspedition.Web.Service;
using Xspedition.Web.ViewModels;

namespace Xspedition.Web.Controllers
{
    public class XspeditionController : Controller
    {
        private readonly ApiService _apiService;

        public XspeditionController()
        {
            _apiService = new ApiService();
        }

        [HttpGet]
        public JsonResult GetCaTimelineModel(int caId)
        {
            HttpClient httpClient = ApiHttpClient.GetHttpClient();

            HttpResponseMessage response = httpClient.GetAsync($"api/xspeditionapi/catimeline/{caId}").Result;
            if (!response.IsSuccessStatusCode)
            {
                return Json(new { Error = "Data Not retrieved successfully." }, JsonRequestBehavior.AllowGet); ;
            }

            string responseContent = response.Content.ReadAsStringAsync().Result;
            CaTimelineViewModel caTimeline  = JsonConvert.DeserializeObject<CaTimelineViewModel>(responseContent);

            return Json(caTimeline, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void SendScrubbingInputData(ScrubCaCommand command)
        {
            PopulateFields(command.FieldsContent, command.Fields);
            foreach (var opt in command.Options)
            {
                PopulateFields(opt.FieldsContent, opt.Fields);
                foreach (var pyt in opt.Payouts)
                {
                    PopulateFields(pyt.FieldsContent, pyt.Fields);
                }
            }

            CaProcessViewModel viewModel = _apiService.Execute(command);

            IHubContext notifHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            if (viewModel != null)
            {
                notifHub.Clients.All.updateProcess(viewModel);
            }

            SummaryModel summaryModel = GetSummaryModel();
            if (summaryModel != null)
            {
                notifHub.Clients.All.updateSummaryModel(summaryModel);
            }
        }

        [HttpGet]
        private SummaryModel GetSummaryModel()
        {
            HttpClient httpClient = ApiHttpClient.GetHttpClient();

            HttpResponseMessage response = httpClient.GetAsync("api/cumulative").Result;
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string responseContent = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<SummaryModel>(responseContent);
        }

        [HttpPost]
        public void SendNotificationInputData(NotifyCommand command)
        {
            CaProcessViewModel viewModel = _apiService.Execute(command);

            IHubContext notifHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            if (viewModel != null)
            {
                notifHub.Clients.All.updateProcess(viewModel);
            }

            SummaryModel summaryModel = GetSummaryModel();
            if (summaryModel != null)
            {
                notifHub.Clients.All.updateSummaryModel(summaryModel);
            }
        }

        [HttpPost]
        public void SendResponseInputData(RespondCommand command)
        {
            CaProcessViewModel viewModel = _apiService.Execute(command);

            IHubContext notifHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            if (viewModel != null)
            {
                notifHub.Clients.All.updateProcess(viewModel);
            }

            SummaryModel summaryModel = GetSummaryModel();
            if (summaryModel != null)
            {
                notifHub.Clients.All.updateSummaryModel(summaryModel);
            }
        }

        [HttpPost]
        public void SendInstructionInputData(InstructCommand command)
        {
            CaProcessViewModel viewModel = _apiService.Execute(command);

            IHubContext notifHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            if (viewModel != null)
            {
                notifHub.Clients.All.updateProcess(viewModel);
            }

            SummaryModel summaryModel = GetSummaryModel();
            if (summaryModel != null)
            {
                notifHub.Clients.All.updateSummaryModel(summaryModel);
            }
        }

        [HttpPost]
        public void SendPaymentInputData(PayCommand command)
        {
            CaProcessViewModel viewModel = _apiService.Execute(command);

            IHubContext notifHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            if (viewModel != null)
            {
                notifHub.Clients.All.updateProcess(viewModel);
            }

            SummaryModel summaryModel = GetSummaryModel();
            if (summaryModel != null)
            {
                notifHub.Clients.All.updateSummaryModel(summaryModel);
            }
        }


        private void PopulateFields(string fieldsContent, Dictionary<int, string> fieldPairs)
        {
            if (!string.IsNullOrWhiteSpace(fieldsContent))
            {
                string[] fields = fieldsContent.Split(';');
                foreach (string field in fields)
                {
                    if (string.IsNullOrWhiteSpace(field))
                        continue;

                    string[] pair = field.Trim().Split('|');
                    int key = int.Parse(pair[0]);
                    string value = pair.Length > 1 ? pair[1] : null;
                    fieldPairs.Add(key, value);
                }
            }
        }
    }
}