using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Razor.Generator;
using Xspedition.Common;
using Xspedition.Web.Entities;
using Xspedition.Web.Entities.Shared;
using Xspedition.Web.Models;
using Xspedition.Web.ViewModels;

namespace Xspedition.Web.Controllers
{
    public class XspeditionApiController : ApiController
    {
        private readonly XspDbContext _context;

        public XspeditionApiController()
        {
            _context = new XspDbContext();
        }

        [HttpGet]
        [Route("api/xspeditionapi/catimeline/{caId}")]
        public IHttpActionResult GetCaTimeline(int caId)
        {
            CaTimelineViewModel caTimelineModel = new CaTimelineViewModel { CaId = caId, TimelineDateFields = new List<TimelineField>(), CaProcessViewModels = new List<CaProcessViewModel>()};

            CaTimeline caTimeline = _context.CaTimeline.FirstOrDefault(ct => ct.CaId == caId);

            if (caTimeline != null)
            {
                caTimelineModel.TimelineRegions = caTimeline;
            }

            AppendCaProcessViewModels(caId, caTimelineModel.CaProcessViewModels);

            AppendRelevantCaTimelineFields(caId, caTimelineModel.TimelineDateFields);

            return Ok(caTimelineModel);
        }

        private void AppendRelevantCaTimelineFields(int caId, List<TimelineField> caTimelineFields)
        {
            List<ScrubbingInfo> relevantDateRecords = _context.ScrubbingInfo.Where(scr => scr.CaId == caId && scr.DateFieldValue != null).ToList();

            caTimelineFields.AddRange(relevantDateRecords.Select(src => new TimelineField
            {
                Content = GetOptionPayoutPrefix(src.OptionNumber, src.PayoutNumber) + _context.FieldRegistry.First(fld => fld.FieldRegistryId == src.FieldRegistryId).FieldDisplay,
                Date = src.DateFieldValue.Value
            }).ToList());
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
                return " ";
            }
        }

        private void AppendCaProcessViewModels(int caId, List<CaProcessViewModel> CaProcessList)
        {
            //SCRUBBING
            List<ScrubbingInfo> scrubbingInfo = _context.ScrubbingInfo.Where(view => view.CaId == caId).ToList();
            List<string> targetDateItems = scrubbingInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.TargetDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> criticalDateItems = scrubbingInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> lateDateItems = scrubbingInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.LateDate).Select(spv => spv.FieldDisplay).ToList();
            List<string> missingItems = scrubbingInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.Missing).Select(spv => spv.FieldDisplay).ToList();
            int processedItemCount = scrubbingInfo.Count(view => view.IsSrubbed && (view.ProcessedDateCategory == ProcessedDateCategory.TargetDate || view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate));
            int totalItemCount = scrubbingInfo.Count;
            
            CaProcessViewModel scrubbingProcess = new CaProcessViewModel(ProcessType.Scrubbing, targetDateItems, criticalDateItems, lateDateItems, missingItems, processedItemCount, totalItemCount, caId);
            AppendCaScrubbedFields(caId, scrubbingProcess.TimelineItems);
            CaProcessList.Add(scrubbingProcess);


            //NOTIFICATION
            List<NotificationInfo> notifInfo = _context.NotificationsInfo.Where(view => view.CaId == caId).ToList();
            targetDateItems = notifInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.TargetDate).Select(spv => spv.FieldDisplay).ToList();
            criticalDateItems = notifInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate).Select(spv => spv.FieldDisplay).ToList();
            lateDateItems = notifInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.LateDate).Select(spv => spv.FieldDisplay).ToList();
            missingItems = notifInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.Missing).Select(spv => spv.FieldDisplay).ToList();
            processedItemCount = notifInfo.Count(view => view.IsSent && (view.ProcessedDateCategory == ProcessedDateCategory.TargetDate || view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate));
            totalItemCount = notifInfo.Count;
            
            CaProcessViewModel notifProcess = new CaProcessViewModel(ProcessType.Notification, targetDateItems, criticalDateItems, lateDateItems, missingItems, processedItemCount, totalItemCount, caId);
            AppendNotificationFields(caId, notifProcess.TimelineItems);
            CaProcessList.Add(notifProcess);

            //RESPONSE
            List<ResponseInfo> responseInfo = _context.ResponsesInfo.Where(view => view.CaId == caId).ToList();
            targetDateItems = responseInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.TargetDate).Select(spv => spv.FieldDisplay).ToList();
            criticalDateItems = responseInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate).Select(spv => spv.FieldDisplay).ToList();
            lateDateItems = responseInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.LateDate).Select(spv => spv.FieldDisplay).ToList();
            missingItems = responseInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.Missing).Select(spv => spv.FieldDisplay).ToList();
            processedItemCount = responseInfo.Count(view => view.IsSubmitted && (view.ProcessedDateCategory == ProcessedDateCategory.TargetDate || view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate));
            totalItemCount = responseInfo.Count;

            CaProcessViewModel responseProcess = new CaProcessViewModel(ProcessType.Response, targetDateItems, criticalDateItems, lateDateItems, missingItems, processedItemCount, totalItemCount, caId);
            AppendResponseFields(caId, responseProcess.TimelineItems);
            CaProcessList.Add(responseProcess);

            //INSTRUCTION
            List<InstructionInfo> instructionInfo = _context.InstructionsInfo.Where(view => view.CaId == caId).ToList();
            targetDateItems = instructionInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.TargetDate).Select(spv => spv.FieldDisplay).ToList();
            criticalDateItems = instructionInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate).Select(spv => spv.FieldDisplay).ToList();
            lateDateItems = instructionInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.LateDate).Select(spv => spv.FieldDisplay).ToList();
            missingItems = instructionInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.Missing).Select(spv => spv.FieldDisplay).ToList();
            processedItemCount = instructionInfo.Count(view => view.IsInstructed && (view.ProcessedDateCategory == ProcessedDateCategory.TargetDate || view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate));
            totalItemCount = instructionInfo.Count;

            CaProcessViewModel intstructionProcess = new CaProcessViewModel(ProcessType.Instruction, targetDateItems, criticalDateItems, lateDateItems, missingItems, processedItemCount, totalItemCount, caId);
            AppendInstructionFields(caId, intstructionProcess.TimelineItems);
            CaProcessList.Add(intstructionProcess);

            //PAYMENT
            List<PaymentInfo> paymentInfo = _context.PaymentsInfo.Where(view => view.CaId == caId).ToList();
            targetDateItems = paymentInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.TargetDate).Select(spv => spv.FieldDisplay).ToList();
            criticalDateItems = paymentInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate).Select(spv => spv.FieldDisplay).ToList();
            lateDateItems = paymentInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.LateDate).Select(spv => spv.FieldDisplay).ToList();
            missingItems = paymentInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.Missing).Select(spv => spv.FieldDisplay).ToList();
            processedItemCount = paymentInfo.Count(view => view.IsSettled && (view.ProcessedDateCategory == ProcessedDateCategory.TargetDate || view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate));
            totalItemCount = paymentInfo.Count;

            CaProcessViewModel paymentProcess = new CaProcessViewModel(ProcessType.Payment, targetDateItems, criticalDateItems, lateDateItems, missingItems, processedItemCount, totalItemCount, caId);
            AppendPaymentFields(caId, paymentProcess.TimelineItems);
            CaProcessList.Add(paymentProcess);
        }


        private void AppendCaScrubbedFields(int caId, List<TimelineField> fields)
        {
            List<ScrubbingInfo> scrubbedRecords = _context.ScrubbingInfo.Where(scr => scr.CaId == caId && scr.ProcessedDate != null && scr.ProcessedDateCategory != ProcessedDateCategory.Missing).ToList();

            fields.AddRange(scrubbedRecords.Select(scr => new TimelineField
            {
                Content = _context.FieldRegistry.First(fld => fld.FieldRegistryId == scr.FieldRegistryId).FieldDisplay + " (CO)",
                Date = scr.ProcessedDate.Value,
                ProcessedDateCategory = scr.ProcessedDateCategory
            }));
        }

        private void AppendNotificationFields(int caId, List<TimelineField> fields)
        {
            List<NotificationInfo> notifRecords = _context.NotificationsInfo.Where(ni => ni.CaId == caId && ni.ProcessedDate != null && ni.ProcessedDateCategory != ProcessedDateCategory.Missing).ToList();

            fields.AddRange(notifRecords.Select(ni => new TimelineField
            {
                Content = "SENT "  + ni.FieldDisplay,
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
    }
}
