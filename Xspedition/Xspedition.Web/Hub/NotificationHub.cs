using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using Xspedition.Common.Commands;
using Xspedition.Common.Dto;
using Xspedition.Web.Service;
using Xspedition.Web.ViewModels;

namespace Xspedition.Web.Hub
{
    [HubName("NotificationHub")]
    public class NotificationHub : Microsoft.AspNet.SignalR.Hub
    {
        private readonly ApiService _apiService;

        public NotificationHub()
        {
            _apiService = new ApiService();
        }

        public void StartSimulation()
        {
            const int sleepTime = 2000;

            Command command = CreateSeccondCAScrubbingEvent();
            CaProcessViewModel viewModel = _apiService.Execute(command);
            if (viewModel != null)
            {
                Clients.All.updateProcess(viewModel);
                Thread.Sleep(sleepTime);
            }

            //command = CreateFirstNotifyEvent();
            //viewModel = _apiService.Execute(command);
            //if (viewModel != null)
            //{
            //    Clients.All.updateProcess(viewModel);
            //    Thread.Sleep(sleepTime);
            //}

            //command = CreateSecondNotifyEvent();
            //viewModel = _apiService.Execute(command);
            //if (viewModel != null)
            //{
            //    Clients.All.updateProcess(viewModel);
            //    Thread.Sleep(sleepTime);
            //}

            //command = CreateFirstResponseEvent();
            //viewModel = _apiService.Execute(command);
            //if (viewModel != null)
            //{
            //    Clients.All.updateProcess(viewModel);
            //    Thread.Sleep(sleepTime);
            //}

            //command = CreateSecondResponseEvent();
            //viewModel = _apiService.Execute(command);
            //if (viewModel != null)
            //{
            //    Clients.All.updateProcess(viewModel);
            //    Thread.Sleep(sleepTime);
            //}

            //command = CreateThridResponseEvent();
            //viewModel = _apiService.Execute(command);
            //if (viewModel != null)
            //{
            //    Clients.All.updateProcess(viewModel);
            //    Thread.Sleep(sleepTime);
            //}

            //command = CreateFirstInstructionEvent();
            //viewModel = _apiService.Execute(command);
            //if (viewModel != null)
            //{
            //    Clients.All.updateProcess(viewModel);
            //    Thread.Sleep(sleepTime);
            //}

            //command = CreateSecondInstructionEvent();
            //viewModel = _apiService.Execute(command);
            //if (viewModel != null)
            //{
            //    Clients.All.updateProcess(viewModel);
            //    Thread.Sleep(sleepTime);
            //}

            //command = CreateFirstPayEvent();
            //viewModel = _apiService.Execute(command);
            //if (viewModel != null)
            //{
            //    Clients.All.updateProcess(viewModel);
            //    Thread.Sleep(sleepTime);
            //}

            //command = CreateSecondPayEvent();
            //viewModel = _apiService.Execute(command);
            //if (viewModel != null)
            //{
            //    Clients.All.updateProcess(viewModel);
            //    Thread.Sleep(sleepTime);
            //}
        }

        private PayCommand CreateFirstPayEvent()
        {
            return new PayCommand
            {
                CaId = 1,
                CaTypeId = 1,
                VolManCho = "V",
                EventDate = DateTime.Now,

                Payments = new List<PaymentDto>
                {
                    new PaymentDto { AccountNumber = "XSP01", OptionNumber = 1, PayoutNumber = 1, IsSettled = false},
                    new PaymentDto { AccountNumber = "XSP01", OptionNumber = 1, PayoutNumber = 2, IsSettled = false},
                    new PaymentDto { AccountNumber = "XSP01", OptionNumber = 2, PayoutNumber = 1, IsSettled = false}
                }
            };
        }

        private PayCommand CreateSecondPayEvent()
        {
            return new PayCommand
            {
                CaId = 1,
                CaTypeId = 1,
                VolManCho = "V",
                EventDate = DateTime.Now,

                Payments = new List<PaymentDto>
                {
                    new PaymentDto { AccountNumber = "XSP01", OptionNumber = 1, PayoutNumber = 1, IsSettled = true},
                    new PaymentDto { AccountNumber = "XSP01", OptionNumber = 1, PayoutNumber = 2, IsSettled = true}
                }
            };
        }

        private InstructCommand CreateFirstInstructionEvent()
        {
            return new InstructCommand
            {
                CaId = 1,
                CaTypeId = 1,
                VolManCho = "V",
                EventDate = DateTime.Now,

                Instructions = new List<InstructionDto>
                {
                    new InstructionDto { AccountNumber = "XSP01", IsInstructed = false},
                    new InstructionDto { AccountNumber = "XSP02", IsInstructed = false},
                    new InstructionDto { AccountNumber = "XSP03", IsInstructed = false}
                }
            };
        }

        private InstructCommand CreateSecondInstructionEvent()
        {
            return new InstructCommand
            {
                CaId = 1,
                CaTypeId = 1,
                VolManCho = "V",
                EventDate = DateTime.Now,

                Instructions = new List<InstructionDto>
                {
                    new InstructionDto { AccountNumber = "XSP01", IsInstructed = true},
                    new InstructionDto { AccountNumber = "XSP03", IsInstructed = true}
                }
            };
        }

        private NotifyCommand CreateFirstNotifyEvent()
        {
            return new NotifyCommand
            {
                CaId = 1,
                CaTypeId = 1,
                VolManCho = "V",
                EventDate = DateTime.Now,


                Notifications = new List<NotificationDto>
                {
                    new NotificationDto { AccountNumber = "XSP01", Recipient = "Alan Harper", IsSent = false},
                    new NotificationDto { AccountNumber = "XSP02", Recipient = "Herb Malwick", IsSent = false},
                    new NotificationDto { AccountNumber = "XSP03", Recipient = "Milos Torbica", IsSent = false}
                }
            };
        }

        private NotifyCommand CreateSecondNotifyEvent()
        {
            return new NotifyCommand
            {
                CaId = 1,
                CaTypeId = 1,
                VolManCho = "V",
                EventDate = DateTime.Now,


                Notifications = new List<NotificationDto>
                {
                    new NotificationDto { AccountNumber = "XSP02", Recipient = "Herb Malwick", IsSent = true},
                    new NotificationDto { AccountNumber = "XSP03", Recipient = "Milos Torbica", IsSent = true}
                }
            };
        }

        private ScrubCaCommand CreateFirstCAScrubbingEvent()
        {
            return new ScrubCaCommand
            {
                CaId = 1,
                CaTypeId = 1,
                EventDate = DateTime.Now,
                VolManCho = "C",
                Fields = new Dictionary<int, string>
                {
                    { 1, "05/01/2017" },
                    { 5, "05/30/2017" },
                    { 10, "05/30/2017" },
                },
                Options = new List<OptionDto>
                {
                    new OptionDto
                    {
                        OptionNumber = 1,
                        OptionTypeId = 1,
                        Fields = new Dictionary<int, string>
                        {
                            { 102, "06/20/2017" },
                            { 103, null },
                            { 106, "05/05/2017" }  
                        },
                        Payouts = new List<PayoutDto>
                        {
                            new PayoutDto
                            {
                                PayoutNumber = 1,
                                PayoutTypeId = 1,
                                Fields = new Dictionary<int, string>
                                {
                                    { 1003, null },
                                    { 1008, "07/10/2017" },
                                    { 1009, "07/25/2017" }
                                }
                            }
                        }
                    },
                    new OptionDto
                    {
                        OptionNumber = 2,
                        OptionTypeId = 1,
                        Fields = new Dictionary<int, string>
                        {
                            { 102, "06/19/2017" },
                            { 103, null },
                            { 106, "05/04/2017" }
                        },
                        Payouts = new List<PayoutDto>
                        {
                            new PayoutDto
                            {
                                PayoutNumber = 1,
                                PayoutTypeId = 1,
                                Fields = new Dictionary<int, string>
                                {
                                    { 1003, null },
                                    { 1008, "07/09/2017" },
                                    { 1009, "07/24/2017" }
                                }
                            },
                            new PayoutDto
                            {
                                PayoutNumber = 2,
                                PayoutTypeId = 1,
                                Fields = new Dictionary<int, string>
                                {
                                    { 1003, null },
                                    { 1008, "07/08/2017" },
                                    { 1009, "07/23/2017" }
                                }
                            }
                        }
                    }
                }
            };
        }

        private ScrubCaCommand CreateSeccondCAScrubbingEvent()
        {
            return new ScrubCaCommand
            {
                CaId = 1,
                CaTypeId = 1,
                EventDate = DateTime.Now,
                VolManCho = "C",
                Fields = null,
                Options = new List<OptionDto>
                {
                    new OptionDto
                    {
                        OptionNumber = 2,
                        OptionTypeId = 1,
                        Fields = new Dictionary<int, string>
                        {
                            { 104, null }
                        },
                        Payouts = null
                    }
                }
            };
        }

        private RespondCommand CreateFirstResponseEvent()
        {
            return new RespondCommand
            {
                CaId = 1,
                CaTypeId = 1,
                VolManCho = "V",
                EventDate = DateTime.Now,

                Responses = new List<ResponseDto>
                {
                    new ResponseDto { AccountNumber = "XSP01", OptionNumber = null, IsSubmitted = false },
                    new ResponseDto { AccountNumber = "XSP02", OptionNumber = null, IsSubmitted = false },
                    new ResponseDto { AccountNumber = "XSP03", OptionNumber = null, IsSubmitted = false }
                }
            };
        }

        private RespondCommand CreateSecondResponseEvent()
        {
            return new RespondCommand
            {
                CaId = 1,
                CaTypeId = 1,
                VolManCho = "V",
                EventDate = DateTime.Now,

                Responses = new List<ResponseDto>
                {
                    new ResponseDto { AccountNumber = "XSP01", OptionNumber = 3, IsSubmitted = true },
                    new ResponseDto { AccountNumber = "XSP02", OptionNumber = 2, IsSubmitted = true }
                }
            };
        }

        private RespondCommand CreateThridResponseEvent()
        {
            return new RespondCommand
            {
                CaId = 1,
                CaTypeId = 1,
                VolManCho = "V",
                EventDate = DateTime.Now.AddDays(27),

                Responses = new List<ResponseDto>
                {
                    new ResponseDto { AccountNumber = "XSP03", OptionNumber = 2, IsSubmitted = true }
                }
            };
        }

    }
}