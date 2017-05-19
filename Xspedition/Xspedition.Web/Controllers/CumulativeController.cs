using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Xspedition.Common;
using Xspedition.Web.Entities;
using Xspedition.Web.Entities.Shared;
using Xspedition.Web.Hub;
using Xspedition.Web.ViewModels;

namespace Xspedition.Web.Controllers
{
    public class CumulativeController : ApiController
    {
        private readonly XspDbContext _context;

        public CumulativeController()
        {
            _context = new XspDbContext();
        }

        [HttpGet]
        [Route("api/cumulative")]
        public IHttpActionResult GetSummaryInformation()
        {
            SummaryModel summaryModel = new SummaryModel();

            List<int> CaIds = _context.CaTimeline.Select(a => a.CaId).ToList();

            foreach (int caId in CaIds)
            {
                SummaryCaModel CorporateActionSummary = CreateSummaryCa(caId);
                summaryModel.CAs.Add(CorporateActionSummary);
            }

            ProcessPrecentageModel scrubingProcessPrecentage = CalculateScrubingProcessPrecentage(0, true);
            summaryModel.SummaryProcessPrecentage.Add(scrubingProcessPrecentage);
            ProcessPrecentageModel notificationProcessPrecentage = CalculateNotificationProcessPrecentage(0, true);
            summaryModel.SummaryProcessPrecentage.Add(notificationProcessPrecentage);
            ProcessPrecentageModel responseProcessPrecentage = CalculateResponseProcessPrecentage(0, true);
            summaryModel.SummaryProcessPrecentage.Add(responseProcessPrecentage);
            ProcessPrecentageModel instructionProcessPrecentage = CalculateInstructionProcessPrecentage(0, true);
            summaryModel.SummaryProcessPrecentage.Add(instructionProcessPrecentage);
            ProcessPrecentageModel paymentProcessPrecentage = CalculatePaymentProcessPrecentage(0, true);
            summaryModel.SummaryProcessPrecentage.Add(paymentProcessPrecentage);

            return Ok(summaryModel);
        }

        private SummaryCaModel CreateSummaryCa(int caId)
        {
            SummaryCaModel summaryCa = new SummaryCaModel();
            summaryCa.CaId = caId;

            ProcessPrecentageModel scrubingProcessPrecentage = CalculateScrubingProcessPrecentage(caId);
            summaryCa.ProcessesPrecentage.Add(scrubingProcessPrecentage);
            ProcessPrecentageModel notificationProcessPrecentage = CalculateNotificationProcessPrecentage(caId);
            summaryCa.ProcessesPrecentage.Add(notificationProcessPrecentage);
            ProcessPrecentageModel ResponseProcessPrecentage = CalculateResponseProcessPrecentage(caId);
            summaryCa.ProcessesPrecentage.Add(ResponseProcessPrecentage);
            ProcessPrecentageModel InstructionProcessPrecentage = CalculateInstructionProcessPrecentage(caId);
            summaryCa.ProcessesPrecentage.Add(InstructionProcessPrecentage);
            ProcessPrecentageModel PaymentProcessPrecentage = CalculatePaymentProcessPrecentage(caId);
            summaryCa.ProcessesPrecentage.Add(PaymentProcessPrecentage);

            var caInfo = _context.ScrubbingInfo.FirstOrDefault(a => a.CaId == caId);
            summaryCa.CaTypeId = caInfo != null ? caInfo.CaTypeId.Value : 0; 

            return summaryCa;
        }

        private ProcessPrecentageModel CalculateScrubingProcessPrecentage(int caId = 0, bool calculateForAllCAs = false)
        {
            ProcessPrecentageModel processPrecentage = new ProcessPrecentageModel();
            processPrecentage.ProcessType = ProcessType.Scrubbing;
            List<ScrubbingInfo> scrubingInfo = null;
            if (calculateForAllCAs)
            {
                scrubingInfo = _context.ScrubbingInfo.ToList();
            } else
            {
                scrubingInfo = _context.ScrubbingInfo.Where(view => view.CaId == caId).ToList();
            }

            decimal totalItems = scrubingInfo.Count;
            if (totalItems > 0)
            {
                decimal targetDateItems = scrubingInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.TargetDate).Select(spv => spv.FieldDisplay).Count();
                decimal criticalDateItems = scrubingInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate).Select(spv => spv.FieldDisplay).Count();
                decimal lateDateItems = scrubingInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.LateDate).Select(spv => spv.FieldDisplay).Count();
                decimal missingItems = scrubingInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.Missing).Select(spv => spv.FieldDisplay).Count();

                processPrecentage.Target = Math.Round(100 * targetDateItems / totalItems);
                processPrecentage.Critical = Math.Round(100 * criticalDateItems / totalItems);
                processPrecentage.Late = Math.Round(100 * lateDateItems / totalItems);
                processPrecentage.Missing = Math.Round(100 * missingItems / totalItems);
            }

            return processPrecentage;
        }

        private ProcessPrecentageModel CalculateNotificationProcessPrecentage(int caId = 0, bool calculateForAllCAs = false)
        {
            ProcessPrecentageModel processPrecentage = new ProcessPrecentageModel();
            processPrecentage.ProcessType = ProcessType.Notification;

            List<NotificationInfo> notificationsInfo = null;
            if (calculateForAllCAs)
            {
                notificationsInfo = _context.NotificationsInfo.ToList();
            }
            else
            {
                notificationsInfo = _context.NotificationsInfo.Where(view => view.CaId == caId).ToList();
            }

            decimal totalItems = notificationsInfo.Count;
            if(totalItems > 0)
            {
                decimal targetDateItems = notificationsInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.TargetDate).Select(spv => spv.FieldDisplay).Count();
                decimal criticalDateItems = notificationsInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate).Select(spv => spv.FieldDisplay).Count();
                decimal lateDateItems = notificationsInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.LateDate).Select(spv => spv.FieldDisplay).Count();
                decimal missingItems = notificationsInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.Missing).Select(spv => spv.FieldDisplay).Count();

                processPrecentage.Target = Math.Round(100 * targetDateItems / totalItems, 2);
                processPrecentage.Critical = Math.Round(100 * criticalDateItems / totalItems, 2);
                processPrecentage.Late = Math.Round(100 * lateDateItems / totalItems, 2);
                processPrecentage.Missing = Math.Round(100 * missingItems / totalItems, 2);
            }

            return processPrecentage;
        }

        private ProcessPrecentageModel CalculateResponseProcessPrecentage(int caId = 0, bool calculateForAllCAs = false)
        {
            ProcessPrecentageModel processPrecentage = new ProcessPrecentageModel();
            processPrecentage.ProcessType = ProcessType.Response;

            List<ResponseInfo> responsesInfo = null;
            if (calculateForAllCAs)
            {
                responsesInfo = _context.ResponsesInfo.ToList();
            }
            else
            {
                responsesInfo = _context.ResponsesInfo.Where(view => view.CaId == caId).ToList();
            }

            decimal totalItems = responsesInfo.Count;
            if (totalItems > 0)
            {
                decimal targetDateItems = responsesInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.TargetDate).Select(spv => spv.FieldDisplay).Count();
                decimal criticalDateItems = responsesInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate).Select(spv => spv.FieldDisplay).Count();
                decimal lateDateItems = responsesInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.LateDate).Select(spv => spv.FieldDisplay).Count();
                decimal missingItems = responsesInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.Missing).Select(spv => spv.FieldDisplay).Count();

                processPrecentage.Target = Math.Round(100 * targetDateItems / totalItems, 2);
                processPrecentage.Critical = Math.Round(100 * criticalDateItems / totalItems, 2);
                processPrecentage.Late = Math.Round(100 * lateDateItems / totalItems, 2);
                processPrecentage.Missing = Math.Round(100 * missingItems / totalItems, 2);
            }

            return processPrecentage;
        }

        private ProcessPrecentageModel CalculateInstructionProcessPrecentage(int caId = 0, bool calculateForAllCAs = false)
        {
            ProcessPrecentageModel processPrecentage = new ProcessPrecentageModel();
            processPrecentage.ProcessType = ProcessType.Instruction;

            List<InstructionInfo> instructionInfo = null;
            if (calculateForAllCAs)
            {
                instructionInfo = _context.InstructionsInfo.ToList();
            }
            else
            {
                instructionInfo = _context.InstructionsInfo.Where(view => view.CaId == caId).ToList();
            }

            decimal totalItems = instructionInfo.Count;
            if (totalItems > 0)
            {
                decimal targetDateItems = instructionInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.TargetDate).Select(spv => spv.FieldDisplay).Count();
                decimal criticalDateItems = instructionInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate).Select(spv => spv.FieldDisplay).Count();
                decimal lateDateItems = instructionInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.LateDate).Select(spv => spv.FieldDisplay).Count();
                decimal missingItems = instructionInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.Missing).Select(spv => spv.FieldDisplay).Count();

                processPrecentage.Target = Math.Round(100 * targetDateItems / totalItems, 2);
                processPrecentage.Critical = Math.Round(100 * criticalDateItems / totalItems, 2);
                processPrecentage.Late = Math.Round(100 * lateDateItems / totalItems, 2);
                processPrecentage.Missing = Math.Round(100 * missingItems / totalItems, 2);
            }

            return processPrecentage;
        }

        private ProcessPrecentageModel CalculatePaymentProcessPrecentage(int caId = 0, bool calculateForAllCAs = false)
        {
            ProcessPrecentageModel processPrecentage = new ProcessPrecentageModel();
            processPrecentage.ProcessType = ProcessType.Payment;

            List<PaymentInfo> paymentInfo = null;
            if (calculateForAllCAs)
            {
                paymentInfo = _context.PaymentsInfo.ToList();
            }
            else
            {
                paymentInfo = _context.PaymentsInfo.Where(view => view.CaId == caId).ToList();
            }

            decimal totalItems = paymentInfo.Count;
            if (totalItems > 0)
            {
                decimal targetDateItems = paymentInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.TargetDate).Select(spv => spv.FieldDisplay).Count();
                decimal criticalDateItems = paymentInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.CriticalDate).Select(spv => spv.FieldDisplay).Count();
                decimal lateDateItems = paymentInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.LateDate).Select(spv => spv.FieldDisplay).Count();
                decimal missingItems = paymentInfo.Where(view => view.ProcessedDateCategory == ProcessedDateCategory.Missing).Select(spv => spv.FieldDisplay).Count();

                processPrecentage.Target = Math.Round(100 * targetDateItems / totalItems, 2);
                processPrecentage.Critical = Math.Round(100 * criticalDateItems / totalItems, 2);
                processPrecentage.Late = Math.Round(100 * lateDateItems / totalItems, 2);
                processPrecentage.Missing = Math.Round(100 * missingItems / totalItems, 2);
            }

            return processPrecentage;
        }
    }
}
