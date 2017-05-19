using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Xspedition.Common;
using Xspedition.Common.Commands;
using Xspedition.Common.Dto;
using Xspedition.Web.Entities;
using Xspedition.Web.Entities.Registries;
using Xspedition.Web.Entities.Shared;
using Xspedition.Web.Entities.System;
using Xspedition.Web.Models;
using Xspedition.Web.ViewModels;

namespace Xspedition.Web.Controllers
{
    public class EventsController : ApiController
    {
        private readonly XspDbContext _context;

        public EventsController()
        {
            _context = new XspDbContext();
        }

        #region SCRUBBING

        [HttpGet]
        [Route("api/events/scrubbing/{caId}")]
        public IHttpActionResult GetScrubbing(int caId)
        {
            List<ScrubbingInfo> scrubbingInfo = _context.ScrubbingInfo.Where(view => view.CaId == caId).ToList();
            List<string> targetDateItems = scrubbingInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.TargetDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> criticalDateItems = scrubbingInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> lateDateItems = scrubbingInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.LateDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> missingItems = scrubbingInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.Missing).Select(spv => spv.FieldDisplay).ToList();

            int processedItemCount = scrubbingInfo.Count(view => view.IsSrubbed && (view.ProcessedDateCategory == ProcessedDateCategory.TargetDate || view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate));
            int totalItemCount = scrubbingInfo.Count;


            CaProcessViewModel scrubbingProcess = new CaProcessViewModel(ProcessType.Scrubbing, targetDateItems, criticalDateItems, lateDateItems, missingItems, processedItemCount, totalItemCount, caId);
            AppendCaScrubbedFields(caId, scrubbingProcess.TimelineItems);
            return Ok(scrubbingProcess);
        }

        [HttpPost]
        [Route("api/events/scrubbing")]
        public IHttpActionResult PostScrubbing([FromBody] ScrubCaCommand command)
        {
            List<ScrubbingInfo> scrubbingViews = _context.ScrubbingInfo.Where(view => view.CaId == command.CaId).ToList();
            if (scrubbingViews.Count == 0)
            {
                CreateScrubbingInfo(command);
            }

            UpdateScrubbingInfo(command);

            return Ok();
        }

        private void CreateScrubbingInfo(ScrubCaCommand command)
        {
            ScrubbingInfo info = null;

            CalculateAndUpdateCaTimeline(command);

            List<int> caFieldRegistryIds = _context
                                            .CaTypeFieldMap
                                            .Where(map => map.CaTypeRegistryId == command.CaTypeId)
                                            .Select(map => map.FieldRegistryId)
                                            .ToList();

            List<FieldRegistry> caFields = _context
                                            .FieldRegistry
                                            .Where(fld => caFieldRegistryIds.Contains(fld.FieldRegistryId))
                                            .ToList();

            foreach (FieldRegistry caField in caFields)
            {
                info = new ScrubbingInfo();
                info.FieldRegistryId = caField.FieldRegistryId;
                info.CaId = command.CaId;
                info.CaTypeId = command.CaTypeId;
                info.OptionNumber = null;
                info.OptionTypeId = null;
                info.PayoutNumber = null;
                info.PayoutTypeId = null;
                info.FieldDisplay = caField.FieldDisplay + " (IN)";
                info.ProcessedDateCategory = ProcessedDateCategory.Missing;
                info.ProcessedDate = null;
                info.IsSrubbed = false;

                _context.ScrubbingInfo.Add(info);
            }

            foreach (OptionDto optionDto in command.Options)
            {
                List<int> optionFieldRegistryIds = _context
                                                    .OptionTypeFieldMap
                                                    .Where(map => map.OptionTypeRegistryId == optionDto.OptionTypeId.Value)
                                                    .Select(map => map.FieldRegistryId)
                                                    .ToList();

                List<FieldRegistry> optionFields = _context
                                                    .FieldRegistry
                                                    .Where(fld => optionFieldRegistryIds.Contains(fld.FieldRegistryId))
                                                    .ToList();

                foreach (FieldRegistry optionField in optionFields)
                {
                    info = new ScrubbingInfo();
                    info.FieldRegistryId = optionField.FieldRegistryId;
                    info.CaId = command.CaId;
                    info.CaTypeId = command.CaTypeId;
                    info.OptionNumber = optionDto.OptionNumber;
                    info.OptionTypeId = optionDto.OptionTypeId.Value;
                    info.PayoutNumber = null;
                    info.PayoutTypeId = null;
                    info.FieldDisplay = "O #" + optionDto.OptionNumber + " - " + optionField.FieldDisplay + " (IN)";
                    info.ProcessedDateCategory = ProcessedDateCategory.Missing;
                    info.ProcessedDate = null;
                    info.IsSrubbed = false;

                    _context.ScrubbingInfo.Add(info);
                }

                foreach (PayoutDto payoutDto in optionDto.Payouts)
                {
                    List<int> payoutFieldRegistryIds = _context
                                                        .PayoutTypeFieldMap
                                                        .Where(map => map.PayoutTypeRegistryId == payoutDto.PayoutTypeId.Value)
                                                        .Select(map => map.FieldRegistryId)
                                                        .ToList();

                    List<FieldRegistry> payoutFields = _context
                                                        .FieldRegistry
                                                        .Where(fld => payoutFieldRegistryIds.Contains(fld.FieldRegistryId))
                                                        .ToList();

                    foreach (FieldRegistry payoutField in payoutFields)
                    {
                        info = new ScrubbingInfo();
                        info.FieldRegistryId = payoutField.FieldRegistryId;
                        info.CaId = command.CaId;
                        info.CaTypeId = command.CaTypeId;
                        info.OptionNumber = optionDto.OptionNumber;
                        info.OptionTypeId = optionDto.OptionTypeId.Value;
                        info.PayoutNumber = payoutDto.PayoutNumber;
                        info.PayoutTypeId = payoutDto.PayoutTypeId.Value;
                        info.FieldDisplay = "O #" + optionDto.OptionNumber + " " + "P #" + payoutDto.PayoutNumber + " - " + payoutField.FieldDisplay + " (IN)";
                        info.ProcessedDateCategory = ProcessedDateCategory.Missing;
                        info.ProcessedDate = null;
                        info.IsSrubbed = false;

                        _context.ScrubbingInfo.Add(info);
                    }
                }
            }

            _context.SaveChanges();
        }

        private void UpdateScrubbingInfo(ScrubCaCommand command)
        {
            ScrubbingInfo info = null;

            if (command.Fields != null)
            {
                foreach (KeyValuePair<int, string> caField in command.Fields)
                {
                    FieldRegistry fieldMetadata = _context.FieldRegistry.FirstOrDefault(f => f.FieldRegistryId == caField.Key);
                    info = _context.ScrubbingInfo.Single(s => s.FieldRegistryId == caField.Key && s.CaId == command.CaId && s.OptionNumber == null && s.PayoutNumber == null);
                    
                    info.PayoutTypeId = command.CaTypeId;
                    info.FieldDisplay = info.FieldDisplay.Substring(0, info.FieldDisplay.Length - 4) + " (CO)";

                    if (fieldMetadata != null && fieldMetadata.FieldType == "DATE" && !string.IsNullOrEmpty(caField.Value))
                    {
                        info.DateFieldValue = DateTime.Parse(caField.Value);
                    }

                    info.ProcessedDateCategory = CalculateProcessedDateCategory(ProcessType.Scrubbing, info.CaId, command.EventDate);
                    info.ProcessedDate = command.EventDate;
                    info.IsSrubbed = true;
                }
            }

            if (command.Options != null)
            {
                foreach (OptionDto optionDto in command.Options)
                {
                    if (optionDto.Fields != null)
                    {
                        foreach (KeyValuePair<int, string> optionField in optionDto.Fields)
                        {
                            FieldRegistry fieldMetadata = _context.FieldRegistry.FirstOrDefault(f => f.FieldRegistryId == optionField.Key);
                            info = _context.ScrubbingInfo.Single(s => s.FieldRegistryId == optionField.Key && s.CaId == command.CaId && s.OptionNumber == optionDto.OptionNumber && s.PayoutNumber == null);

                            info.PayoutTypeId = GetOptionTypeId(command.CaId, optionDto.OptionNumber);
                            info.FieldDisplay = info.FieldDisplay.Substring(0, info.FieldDisplay.Length - 4) + " (CO)";

                            if (fieldMetadata != null && fieldMetadata.FieldType == "DATE" && !string.IsNullOrEmpty(optionField.Value))
                            {
                                info.DateFieldValue = DateTime.Parse(optionField.Value);
                            }

                            info.ProcessedDateCategory = CalculateProcessedDateCategory(ProcessType.Scrubbing, info.CaId, command.EventDate);
                            info.ProcessedDate = command.EventDate;
                            info.IsSrubbed = true;
                        }
                    }

                    if (optionDto.Payouts != null)
                    {
                        foreach (PayoutDto payoutDto in optionDto.Payouts)
                        {
                            if (payoutDto.Fields != null)
                            {
                                foreach (KeyValuePair<int, string> payoutField in payoutDto.Fields)
                                {
                                    FieldRegistry fieldMetadata = _context.FieldRegistry.FirstOrDefault(f => f.FieldRegistryId == payoutField.Key);
                                    info = _context.ScrubbingInfo.Single(s => s.FieldRegistryId == payoutField.Key && s.CaId == command.CaId && s.OptionNumber == optionDto.OptionNumber && s.PayoutNumber == payoutDto.PayoutNumber);

                                    info.PayoutTypeId = GetPayoutTypeId(command.CaId, optionDto.OptionNumber, payoutDto.PayoutNumber);
                                    info.FieldDisplay = info.FieldDisplay.Substring(0, info.FieldDisplay.Length - 4) + " (CO)";

                                    if (fieldMetadata != null && fieldMetadata.FieldType == "DATE" && !string.IsNullOrEmpty(payoutField.Value))
                                    {
                                        info.DateFieldValue = DateTime.Parse(payoutField.Value);
                                    }

                                    info.ProcessedDateCategory = CalculateProcessedDateCategory(ProcessType.Scrubbing, info.CaId, command.EventDate);
                                    info.ProcessedDate = command.EventDate;
                                    info.IsSrubbed = true;
                                }
                            }
                        }
                    }
                }
            }

            _context.SaveChanges();
        }

        public void CalculateAndUpdateCaTimeline(ScrubCaCommand command)
        {
            CaTimeline caTimeline = _context.CaTimeline.FirstOrDefault(ct => ct.CaId == command.CaId);

            if (caTimeline != null)
            {
                _context.CaTimeline.Remove(caTimeline);
                _context.SaveChanges();
            }

            CaTimeline newTimeline = new CaTimeline();
            newTimeline.CaId = command.CaId;

            //Scrubbing Process Dates
            CaTypeDateConfiguration caTypeStartDateConfiguration = _context.DatesConfigurations.FirstOrDefault(dc => dc.CaType == command.CaTypeId && dc.ProcessType == ProcessType.Scrubbing && dc.DateType.Equals("S", StringComparison.CurrentCultureIgnoreCase));
            CaTypeDateConfiguration caTypeTargetDateConfiguration = _context.DatesConfigurations.FirstOrDefault(dc => dc.CaType == command.CaTypeId && dc.ProcessType == ProcessType.Scrubbing && dc.DateType.Equals("T", StringComparison.CurrentCultureIgnoreCase));
            CaTypeDateConfiguration caTypeCriticalDateConfiguration = _context.DatesConfigurations.FirstOrDefault(dc => dc.CaType == command.CaTypeId && dc.ProcessType == ProcessType.Scrubbing && dc.DateType.Equals("C", StringComparison.CurrentCultureIgnoreCase));

            newTimeline.ScrubbingStart = DateTime.Parse(command.Fields[caTypeStartDateConfiguration.FieldRegistryId]).AddDays(caTypeStartDateConfiguration.DateOffset);
            newTimeline.ScrubbingTarget = DateTime.Parse(command.Fields[caTypeTargetDateConfiguration.FieldRegistryId]).AddDays(caTypeTargetDateConfiguration.DateOffset);
            newTimeline.ScrubbingCritical = DateTime.Parse(command.Fields[caTypeCriticalDateConfiguration.FieldRegistryId]).AddDays(caTypeCriticalDateConfiguration.DateOffset);

            //Notification Process Dates
            caTypeStartDateConfiguration = _context.DatesConfigurations.FirstOrDefault(dc => dc.CaType == command.CaTypeId && dc.ProcessType == ProcessType.Notification && dc.DateType.Equals("S", StringComparison.CurrentCultureIgnoreCase));
            caTypeTargetDateConfiguration = _context.DatesConfigurations.FirstOrDefault(dc => dc.CaType == command.CaTypeId && dc.ProcessType == ProcessType.Notification && dc.DateType.Equals("T", StringComparison.CurrentCultureIgnoreCase));
            caTypeCriticalDateConfiguration = _context.DatesConfigurations.FirstOrDefault(dc => dc.CaType == command.CaTypeId && dc.ProcessType == ProcessType.Notification && dc.DateType.Equals("C", StringComparison.CurrentCultureIgnoreCase));

            newTimeline.NotificationStart = DateTime.Parse(command.Fields[caTypeStartDateConfiguration.FieldRegistryId]).AddDays(caTypeStartDateConfiguration.DateOffset);
            newTimeline.NotificationTarget = DateTime.Parse(command.Fields[caTypeTargetDateConfiguration.FieldRegistryId]).AddDays(caTypeTargetDateConfiguration.DateOffset);
            newTimeline.NotificationCritical = DateTime.Parse(command.Fields[caTypeCriticalDateConfiguration.FieldRegistryId]).AddDays(caTypeCriticalDateConfiguration.DateOffset);

            if (command.VolManCho.Equals("M", StringComparison.CurrentCultureIgnoreCase))
            {
                newTimeline.ResponseStart = null;
                newTimeline.ResponseTarget = null;
                newTimeline.ResponseCritical = null;

                newTimeline.InstructionStart = null;
                newTimeline.InstructionTarget = null;
                newTimeline.InstructionCritical = null;
            }
            else
            {
                //Response Process Dates
                caTypeStartDateConfiguration = _context.DatesConfigurations.FirstOrDefault(dc => dc.CaType == command.CaTypeId && dc.ProcessType == ProcessType.Response && dc.DateType.Equals("S", StringComparison.CurrentCultureIgnoreCase));
                caTypeTargetDateConfiguration = _context.DatesConfigurations.FirstOrDefault(dc => dc.CaType == command.CaTypeId && dc.ProcessType == ProcessType.Response && dc.DateType.Equals("T", StringComparison.CurrentCultureIgnoreCase));
                caTypeCriticalDateConfiguration = _context.DatesConfigurations.FirstOrDefault(dc => dc.CaType == command.CaTypeId && dc.ProcessType == ProcessType.Response && dc.DateType.Equals("C", StringComparison.CurrentCultureIgnoreCase));

                List<DateTime> optionDates = new List<DateTime>();
                command.Options.ForEach(opt => FindDate(opt.Fields, caTypeStartDateConfiguration.FieldRegistryId, optionDates));
                newTimeline.ResponseStart = optionDates.Min().AddDays(caTypeStartDateConfiguration.DateOffset);

                optionDates = new List<DateTime>();
                command.Options.ForEach(opt => FindDate(opt.Fields, caTypeTargetDateConfiguration.FieldRegistryId, optionDates));
                newTimeline.ResponseTarget = optionDates.Min().AddDays(caTypeTargetDateConfiguration.DateOffset);

                optionDates = new List<DateTime>();
                command.Options.ForEach(opt => FindDate(opt.Fields, caTypeCriticalDateConfiguration.FieldRegistryId, optionDates));
                newTimeline.ResponseCritical = optionDates.Min().AddDays(caTypeCriticalDateConfiguration.DateOffset);

                //Instruction Process Dates
                caTypeStartDateConfiguration = _context.DatesConfigurations.FirstOrDefault(dc => dc.CaType == command.CaTypeId && dc.ProcessType == ProcessType.Instruction && dc.DateType.Equals("S", StringComparison.CurrentCultureIgnoreCase));
                caTypeTargetDateConfiguration = _context.DatesConfigurations.FirstOrDefault(dc => dc.CaType == command.CaTypeId && dc.ProcessType == ProcessType.Instruction && dc.DateType.Equals("T", StringComparison.CurrentCultureIgnoreCase));
                caTypeCriticalDateConfiguration = _context.DatesConfigurations.FirstOrDefault(dc => dc.CaType == command.CaTypeId && dc.ProcessType == ProcessType.Instruction && dc.DateType.Equals("C", StringComparison.CurrentCultureIgnoreCase));

                optionDates = new List<DateTime>();
                command.Options.ForEach(opt => FindDate(opt.Fields, caTypeStartDateConfiguration.FieldRegistryId, optionDates));
                newTimeline.InstructionStart = optionDates.Min().AddDays(caTypeStartDateConfiguration.DateOffset);

                optionDates = new List<DateTime>();
                command.Options.ForEach(opt => FindDate(opt.Fields, caTypeTargetDateConfiguration.FieldRegistryId, optionDates));
                newTimeline.InstructionTarget = optionDates.Min().AddDays(caTypeTargetDateConfiguration.DateOffset);

                optionDates = new List<DateTime>();
                command.Options.ForEach(opt => FindDate(opt.Fields, caTypeCriticalDateConfiguration.FieldRegistryId, optionDates));
                newTimeline.InstructionCritical = optionDates.Min().AddDays(caTypeCriticalDateConfiguration.DateOffset);
            }

            //Payment Process Dates
            caTypeStartDateConfiguration = _context.DatesConfigurations.FirstOrDefault(dc => dc.CaType == command.CaTypeId && dc.ProcessType == ProcessType.Payment && dc.DateType.Equals("S", StringComparison.CurrentCultureIgnoreCase));
            caTypeTargetDateConfiguration = _context.DatesConfigurations.FirstOrDefault(dc => dc.CaType == command.CaTypeId && dc.ProcessType == ProcessType.Payment && dc.DateType.Equals("T", StringComparison.CurrentCultureIgnoreCase));
            caTypeCriticalDateConfiguration = _context.DatesConfigurations.FirstOrDefault(dc => dc.CaType == command.CaTypeId && dc.ProcessType == ProcessType.Payment && dc.DateType.Equals("C", StringComparison.CurrentCultureIgnoreCase));

            List<DateTime> payoutDates = new List<DateTime>();
            command.Options.ForEach(opt => opt.Payouts.ForEach(pt => FindDate(pt.Fields, caTypeStartDateConfiguration.FieldRegistryId, payoutDates)));
            newTimeline.PaymentStart = payoutDates.Min().AddDays(caTypeStartDateConfiguration.DateOffset);

            payoutDates = new List<DateTime>();
            command.Options.ForEach(opt => opt.Payouts.ForEach(pt => FindDate(pt.Fields, caTypeTargetDateConfiguration.FieldRegistryId, payoutDates)));
            newTimeline.PaymentTarget = payoutDates.Min().AddDays(caTypeTargetDateConfiguration.DateOffset);

            payoutDates = new List<DateTime>();
            command.Options.ForEach(opt => opt.Payouts.ForEach(pt => FindDate(pt.Fields, caTypeCriticalDateConfiguration.FieldRegistryId, payoutDates)));
            newTimeline.PaymentCritical = payoutDates.Min().AddDays(caTypeCriticalDateConfiguration.DateOffset);

            _context.CaTimeline.Add(newTimeline);
            _context.SaveChanges();
        }

        private void FindDate(Dictionary<int, string> fields, int fieldRegistryId, List<DateTime> dates)
        {
            if (fields.ContainsKey(fieldRegistryId))
            {
                dates.Add(DateTime.Parse(fields[fieldRegistryId]));
            }
        }

        private int GetOptionTypeId(int caId, int optionNumber)
        {
            return _context.ScrubbingInfo.First(s => s.CaId == caId && s.OptionNumber == optionNumber && s.OptionTypeId != null).OptionTypeId.Value;
        }

        private int GetPayoutTypeId(int caId, int optionNumber, int payoutNumber)
        {
            return _context.ScrubbingInfo.First(s => s.CaId == caId && s.OptionNumber == optionNumber && s.PayoutNumber == payoutNumber && s.PayoutTypeId != null).PayoutTypeId.Value;
        }

        #endregion SCRUBBING

        #region NOTIFICATIONS

        [HttpGet]
        [Route("api/events/notifications/{caId}")]
        public IHttpActionResult GetNotifications(int caId)
        {
            List<NotificationInfo> notifications = _context.NotificationsInfo.Where(view => view.CaId == caId).ToList();
            List<string> targetDateItems = notifications.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.TargetDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> criticalDateItems = notifications.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> lateDateItems = notifications.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.LateDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> missingItems = notifications.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.Missing).Select(spv => spv.FieldDisplay).ToList();
            int processedItemCount = notifications.Count(view => view.IsSent && (view.ProcessedDateCategory == ProcessedDateCategory.TargetDate || view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate));
            int totalItemCount = notifications.Count;

            CaProcessViewModel notifProcess = new CaProcessViewModel(ProcessType.Notification, targetDateItems, criticalDateItems, lateDateItems, missingItems, processedItemCount, totalItemCount, caId);
            AppendNotificationFields(caId, notifProcess.TimelineItems);
            return Ok(notifProcess);
        }

        [HttpPost]
        [Route("api/events/notifications")]
        public IHttpActionResult PostNotifications([FromBody] NotifyCommand command)
        {
            List<NotificationInfo> notifications = _context.NotificationsInfo.Where(view => view.CaId == command.CaId).ToList();
            if (notifications.Count == 0)
            {
                CreateNotificationsInfo(command);
            }
            else
            {
                UpdateNotificationsInfo(command);
            }

            return Ok();
        }

        private void CreateNotificationsInfo(NotifyCommand command)
        {
            foreach (NotificationDto notificationDto in command.Notifications)
            {
                NotificationInfo notification = new NotificationInfo();

                notification.CaId = command.CaId;
                notification.CaTypeId = command.CaTypeId;
                notification.VolManCho = command.VolManCho;
                notification.AccountNumber = notificationDto.AccountNumber;
                notification.Recipient = notificationDto.Recipient;
                notification.FieldDisplay = notificationDto.Recipient + " (" + notificationDto.AccountNumber + ")";

                if (notificationDto.IsSent)
                {
                    notification.ProcessedDateCategory = CalculateProcessedDateCategory(ProcessType.Notification, command.CaId, command.EventDate);
                    notification.IsSent = true;
                    notification.ProcessedDate = command.EventDate;
                }
                else
                {
                    notification.ProcessedDateCategory = ProcessedDateCategory.Missing;
                    notification.IsSent = false;
                    notification.ProcessedDate = null;
                }

                _context.NotificationsInfo.Add(notification);
            }
            _context.SaveChanges();
        }

        private void UpdateNotificationsInfo(NotifyCommand command)
        {
            foreach (NotificationDto notificationDto in command.Notifications)
            {
                NotificationInfo notification = _context.NotificationsInfo.FirstOrDefault(notif => notif.CaId == command.CaId && notif.AccountNumber == notificationDto.AccountNumber && notif.Recipient == notificationDto.Recipient);

                if (notification == null)
                {
                    continue;
                }

                if (notificationDto.IsSent)
                {
                    notification.ProcessedDateCategory = CalculateProcessedDateCategory(ProcessType.Notification, command.CaId, command.EventDate);
                    notification.IsSent = true;
                    notification.ProcessedDate = command.EventDate;
                }
                else
                {
                    notification.ProcessedDateCategory = ProcessedDateCategory.Missing;
                    notification.IsSent = false;
                    notification.ProcessedDate = null;
                }
            }

            _context.SaveChanges();
        }

        #endregion

        #region RESPONSES

        [HttpGet]
        [Route("api/events/responses/{caId}")]
        public IHttpActionResult GetResponses(int caId)
        {
            List<ResponseInfo> responses = _context.ResponsesInfo.Where(view => view.CaId == caId).ToList();
            List<string> targetDateItems = responses.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.TargetDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> criticalDateItems = responses.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> lateDateItems = responses.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.LateDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> missingItems = responses.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.Missing).Select(spv => spv.FieldDisplay).ToList();
            int processedItemCount = responses.Count(view => view.IsSubmitted && (view.ProcessedDateCategory == ProcessedDateCategory.TargetDate || view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate));
            int totalItemCount = responses.Count;
            
            CaProcessViewModel responseProcess = new CaProcessViewModel(ProcessType.Response, targetDateItems, criticalDateItems, lateDateItems, missingItems, processedItemCount, totalItemCount, caId);
            AppendResponseFields(caId, responseProcess.TimelineItems);
            return Ok(responseProcess);
        }

        [HttpPost]
        [Route("api/events/responses")]
        public IHttpActionResult PostResponses([FromBody] RespondCommand command)
        {
            List<ResponseInfo> responses = _context.ResponsesInfo.Where(view => view.CaId == command.CaId).ToList();
            if (responses.Count == 0)
            {
                CreateResponseInfo(command);
            }
            else
            {
                UpdateResponseInfo(command);
            }

            return Ok();
        }

        private void CreateResponseInfo(RespondCommand command)
        {
            if (command.VolManCho != null && command.VolManCho.Equals("M", StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }

            foreach (ResponseDto responseDto in command.Responses)
            {
                ResponseInfo response = new ResponseInfo();

                response.CaId = command.CaId;
                response.CaTypeId = command.CaTypeId;
                response.VolManCho = command.VolManCho;
                response.AccountNumber = responseDto.AccountNumber;
                response.FieldDisplay = responseDto.AccountNumber;

                if (responseDto.IsSubmitted)
                {
                    response.ProcessedDateCategory = CalculateProcessedDateCategory(ProcessType.Response, command.CaId, command.EventDate);
                    response.IsSubmitted = true;
                    response.OptionNumber = responseDto.OptionNumber.Value;
                    response.FieldDisplay += " (Option #" + response.OptionNumber + ")";
                    response.ProcessedDate = command.EventDate;
                }
                else
                {
                    response.ProcessedDateCategory = ProcessedDateCategory.Missing;
                    response.IsSubmitted = false;
                    response.ProcessedDate = null;
                }

                _context.ResponsesInfo.Add(response);
            }

            _context.SaveChanges();
        }

        private void UpdateResponseInfo(RespondCommand command)
        {
            if (command.VolManCho != null && command.VolManCho.Equals("M", StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }

            foreach (ResponseDto responseDto in command.Responses)
            {
                ResponseInfo response = _context.ResponsesInfo.FirstOrDefault(rsp => rsp.CaId == command.CaId && rsp.AccountNumber == responseDto.AccountNumber && rsp.OptionNumber == null);

                if (response == null)
                {
                    continue;
                }

                if (responseDto.IsSubmitted)
                {
                    response.ProcessedDateCategory = CalculateProcessedDateCategory(ProcessType.Response, command.CaId, command.EventDate);
                    response.IsSubmitted = true;
                    response.OptionNumber = responseDto.OptionNumber.Value;
                    response.FieldDisplay = response.AccountNumber + " (Option #" + response.OptionNumber + ")";
                    response.ProcessedDate = command.EventDate;
                }
                else
                {
                    response.ProcessedDateCategory = ProcessedDateCategory.Missing;
                    response.IsSubmitted = false;
                    response.ProcessedDate = null;
                }
            }

            _context.SaveChanges();
        }

        #endregion

        #region INSTRUCTIONS

        [HttpGet]
        [Route("api/events/instructions/{caId}")]
        public IHttpActionResult GetInstructions(int caId)
        {
            List<InstructionInfo> instructions = _context.InstructionsInfo.Where(view => view.CaId == caId).ToList();
            List<string> targetDateItems = instructions.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.TargetDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> criticalDateItems = instructions.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> lateDateItems = instructions.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.LateDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> missingItems = instructions.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.Missing).Select(spv => spv.FieldDisplay).ToList();
            int processedItemCount = instructions.Count(view => view.IsInstructed && (view.ProcessedDateCategory == ProcessedDateCategory.TargetDate || view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate));
            int totalItemCount = instructions.Count;

            CaProcessViewModel intstructionProcess = new CaProcessViewModel(ProcessType.Instruction, targetDateItems, criticalDateItems, lateDateItems, missingItems, processedItemCount, totalItemCount, caId);
            AppendInstructionFields(caId, intstructionProcess.TimelineItems);
            return Ok(intstructionProcess);
        }

        [HttpPost]
        [Route("api/events/instructions")]
        public IHttpActionResult PostInstructions([FromBody] InstructCommand command)
        {
            List<InstructionInfo> instructions = _context.InstructionsInfo.Where(view => view.CaId == command.CaId).ToList();
            if (instructions.Count == 0)
            {
                CreateInstructionsInfo(command);
            }
            else
            {
                UpdateInstructionInfo(command);
            }

            return Ok();
        }

        private void CreateInstructionsInfo(InstructCommand command)
        {
            if (command.VolManCho != null && command.VolManCho.Equals("M", StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }

            foreach (InstructionDto instructionDto in command.Instructions)
            {
                InstructionInfo instruction = new InstructionInfo();

                instruction.CaId = command.CaId;
                instruction.CaTypeId = command.CaTypeId;
                instruction.VolManCho = command.VolManCho;
                instruction.AccountNumber = instructionDto.AccountNumber;
                instruction.FieldDisplay = instructionDto.AccountNumber;

                if (instructionDto.IsInstructed)
                {
                    instruction.ProcessedDateCategory = CalculateProcessedDateCategory(ProcessType.Instruction, command.CaId, command.EventDate);
                    instruction.IsInstructed = true;
                    instruction.ProcessedDate = command.EventDate;
                }
                else
                {
                    instruction.ProcessedDateCategory = ProcessedDateCategory.Missing;
                    instruction.IsInstructed = false;
                    instruction.ProcessedDate = null;
                }

                _context.InstructionsInfo.Add(instruction);
            }

            _context.SaveChanges();
        }

        private void UpdateInstructionInfo(InstructCommand command)
        {
            if (command.VolManCho != null && command.VolManCho.Equals("M", StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }

            foreach (InstructionDto instructionDto in command.Instructions)
            {
                InstructionInfo instruction = _context.InstructionsInfo.FirstOrDefault(ins => ins.CaId == command.CaId && ins.AccountNumber == instructionDto.AccountNumber);

                if (instruction == null)
                {
                    continue;
                }

                if (instructionDto.IsInstructed)
                {
                    instruction.ProcessedDateCategory = CalculateProcessedDateCategory(ProcessType.Instruction, command.CaId, command.EventDate);
                    instruction.IsInstructed = true;
                    instruction.ProcessedDate = command.EventDate;
                }
                else
                {
                    instruction.ProcessedDateCategory = ProcessedDateCategory.Missing;
                    instruction.IsInstructed = false;
                    instruction.ProcessedDate = null;
                }
            }

            _context.SaveChanges();
        }

        #endregion

        #region PAYMENTS

        [HttpGet]
        [Route("api/events/payments/{caId}")]
        public IHttpActionResult GetPayments(int caId)
        {
            List<PaymentInfo> payments = _context.PaymentsInfo.Where(view => view.CaId == caId).ToList();
            List<string> targetDateItems = payments.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.TargetDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> criticalDateItems = payments.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> lateDateItems = payments.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.LateDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> missingItems = payments.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.Missing).Select(spv => spv.FieldDisplay).ToList();
            int processedItemCount = payments.Count(view => view.IsSettled && (view.ProcessedDateCategory == ProcessedDateCategory.TargetDate || view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate));
            int totalItemCount = payments.Count;
            
            CaProcessViewModel paymentProcess = new CaProcessViewModel(ProcessType.Payment, targetDateItems, criticalDateItems, lateDateItems, missingItems, processedItemCount, totalItemCount, caId);
            AppendPaymentFields(caId, paymentProcess.TimelineItems);
            return Ok(paymentProcess);
        }

        [HttpPost]
        [Route("api/events/payments")]
        public IHttpActionResult PostPayments([FromBody] PayCommand command)
        {
            List<PaymentInfo> payments = _context.PaymentsInfo.Where(view => view.CaId == command.CaId).ToList();
            if (payments.Count == 0)
            {
                CreatePaymentInfo(command);
            }
            else
            {
                UpdatePaymentInfo(command);
            }

            return Ok();
        }

        private void CreatePaymentInfo(PayCommand command)
        {
            foreach (PaymentDto paymentDto in command.Payments)
            {
                PaymentInfo payment = new PaymentInfo();

                payment.CaId = command.CaId;
                payment.CaTypeId = command.CaTypeId;
                payment.VolManCho = command.VolManCho;
                payment.AccountNumber = paymentDto.AccountNumber;
                payment.FieldDisplay = "O #" + paymentDto.OptionNumber + " " + "P #" + paymentDto.PayoutNumber + " - " + paymentDto.AccountNumber;
                payment.OptionNumber = paymentDto.OptionNumber;
                payment.PayoutNumber = paymentDto.PayoutNumber;

                if (paymentDto.IsSettled)
                {
                    payment.ProcessedDateCategory = CalculateProcessedDateCategory(ProcessType.Payment, command.CaId, command.EventDate);
                    payment.IsSettled = true;
                    payment.ProcessedDate = command.EventDate;
                }
                else
                {
                    payment.ProcessedDateCategory = ProcessedDateCategory.Missing;
                    payment.IsSettled = false;
                    payment.ProcessedDate = null;
                }

                _context.PaymentsInfo.Add(payment);
            }

            _context.SaveChanges();
        }

        private void UpdatePaymentInfo(PayCommand command)
        {
            foreach (PaymentDto paymentDto in command.Payments)
            {
                PaymentInfo payment = _context.PaymentsInfo.FirstOrDefault(pay => pay.CaId == command.CaId && pay.AccountNumber == paymentDto.AccountNumber && pay.OptionNumber == paymentDto.OptionNumber && pay.PayoutNumber == paymentDto.PayoutNumber);

                if (payment == null)
                {
                    continue;
                }

                if (paymentDto.IsSettled)
                {
                    payment.ProcessedDateCategory = CalculateProcessedDateCategory(ProcessType.Payment, command.CaId, command.EventDate);
                    payment.IsSettled = true;
                    payment.ProcessedDate = command.EventDate;
                }
                else
                {
                    payment.ProcessedDateCategory = ProcessedDateCategory.Missing;
                    payment.IsSettled = false;
                    payment.ProcessedDate = null;
                }
            }

            _context.SaveChanges();
        }

        #endregion

        #region COMMON METHODS
        private ProcessedDateCategory CalculateProcessedDateCategory(ProcessType processType, int caId, DateTime eventDate)
        {
            DateTime? targetDate = new DateTime();
            DateTime? criticalDate = new DateTime();
            CaTimeline caTimeline = _context.CaTimeline.SingleOrDefault(ctv => ctv.CaId == caId);

            if (caTimeline == null)
            {
                return ProcessedDateCategory.Missing;
            }

            switch (processType)
            {
                case ProcessType.Scrubbing:
                    targetDate = caTimeline.ScrubbingTarget;
                    criticalDate = caTimeline.ScrubbingCritical;
                    break;
                case ProcessType.Notification:
                    targetDate = caTimeline.NotificationTarget;
                    criticalDate = caTimeline.NotificationCritical;
                    break;
                case ProcessType.Response:
                    targetDate = caTimeline.ResponseTarget;
                    criticalDate = caTimeline.ResponseCritical;
                    break;
                case ProcessType.Instruction:
                    targetDate = caTimeline.InstructionTarget;
                    criticalDate = caTimeline.InstructionCritical;
                    break;
                case ProcessType.Payment:
                    targetDate = caTimeline.PaymentTarget;
                    criticalDate = caTimeline.PaymentCritical;
                    break;
            }

            if (!targetDate.HasValue || !criticalDate.HasValue)
            {
                return ProcessedDateCategory.Missing;
            }

            return GetProcessedDateCategory(eventDate, targetDate.Value, criticalDate.Value);
        }

        private ProcessedDateCategory GetProcessedDateCategory(DateTime eventDate, DateTime targetDate, DateTime criticalDate)
        {
            if (eventDate <= targetDate)
            {
                return ProcessedDateCategory.TargetDate;
            }

            if (eventDate <= criticalDate)
            {
                return ProcessedDateCategory.CriticalDate;
            }

            return ProcessedDateCategory.LateDate;
        }

        private void AppendCaScrubbedFields(int caId, List<TimelineField> fields)
        {
            List<ScrubbingInfo> scrubbedRecords = _context.ScrubbingInfo.Where(scr => scr.CaId == caId && scr.ProcessedDate != null && scr.ProcessedDateCategory != ProcessedDateCategory.Missing).ToList();

            fields.AddRange(scrubbedRecords.Select(scr => new TimelineField
            {
                Content = GetOptionPayoutPrefix(scr.OptionNumber, scr.PayoutNumber)  + _context.FieldRegistry.First(fld => fld.FieldRegistryId == scr.FieldRegistryId).FieldDisplay + " (CO)",
                Date = scr.ProcessedDate.Value,
                ProcessedDateCategory = scr.ProcessedDateCategory
            }));
        }

        private void AppendNotificationFields(int caId, List<TimelineField> fields)
        {
            List<NotificationInfo> notifRecords = _context.NotificationsInfo.Where(ni => ni.CaId == caId && ni.ProcessedDate != null && ni.ProcessedDateCategory != ProcessedDateCategory.Missing).ToList();

            fields.AddRange(notifRecords.Select(ni => new TimelineField
            {
                Content = "SENT " + ni.FieldDisplay,
                Date = ni.ProcessedDate.Value,
                ProcessedDateCategory = ni.ProcessedDateCategory
            }));
        }

        private void AppendResponseFields(int caId, List<TimelineField> fields)
        {
            List<ResponseInfo> responseRecords = _context.ResponsesInfo.Where(ri => ri.CaId == caId && ri.ProcessedDate != null && ri.ProcessedDateCategory != ProcessedDateCategory.Missing).ToList();

            fields.AddRange(responseRecords.Select(ri => new TimelineField
            {
                Content = ri.FieldDisplay,
                Date = ri.ProcessedDate.Value,
                ProcessedDateCategory = ri.ProcessedDateCategory
            }));
        }

        private void AppendInstructionFields(int caId, List<TimelineField> fields)
        {
            List<InstructionInfo> instructionRecords = _context.InstructionsInfo.Where(ii => ii.CaId == caId && ii.ProcessedDate != null && ii.ProcessedDateCategory != ProcessedDateCategory.Missing).ToList();

            fields.AddRange(instructionRecords.Select(ii => new TimelineField
            {
                Content = ii.FieldDisplay,
                Date = ii.ProcessedDate.Value,
                ProcessedDateCategory = ii.ProcessedDateCategory
            }));
        }

        private void AppendPaymentFields(int caId, List<TimelineField> fields)
        {
            List<PaymentInfo> paymentRecords = _context.PaymentsInfo.Where(pi => pi.CaId == caId && pi.ProcessedDate != null && pi.ProcessedDateCategory != ProcessedDateCategory.Missing).ToList();

            fields.AddRange(paymentRecords.Select(pr => new TimelineField
            {
                Content = pr.FieldDisplay,
                Date = pr.ProcessedDate.Value,
                ProcessedDateCategory = pr.ProcessedDateCategory
            }));
        }
        
        private string GetOptionPayoutPrefix(int? optionNumber, int? payoutNumber)
        {
            if (payoutNumber.HasValue)
            {
                return "O #" + optionNumber + " P #" + payoutNumber + " ";
            }
            else if (optionNumber.HasValue)
            {
                return "O #" + optionNumber + " ";
            }
            else
            {
                return "";
            }
        }

        #endregion
    }
}
