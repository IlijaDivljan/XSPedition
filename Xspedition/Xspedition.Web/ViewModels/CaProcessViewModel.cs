using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xspedition.Web.Entities.Shared;
using Xspedition.Web.Models;

namespace Xspedition.Web.ViewModels
{
    public class CaProcessViewModel
    {
        public CaProcessViewModel(ProcessType processType, List<string> targetDateItems, List<string> criticalDateItems, List<string> lateDateItems, List<string> missingItems, int processedItemCount, int totalItemCount, int caId)
        {
            switch (processType)
            {
                case ProcessType.Scrubbing:
                    this.id = "scrub";
                    this.title = "Scrubbing";
                    this.processActionName = "Scrubbed";
                    this.processType = ProcessType.Scrubbing;
                    break;
                case ProcessType.Notification:
                    this.id = "notif";
                    this.title = "Notification";
                    this.processActionName = "Sent";
                    this.processType = ProcessType.Notification;
                    break;
                case ProcessType.Response:
                    this.id = "respo";
                    this.title = "Response";
                    this.processActionName = "Submitted";
                    this.processType = ProcessType.Response;
                    break;
                case ProcessType.Instruction:
                    this.id = "instr";
                    this.title = "Instruction";
                    this.processActionName = "Instructed";
                    this.processType = ProcessType.Instruction;
                    break;
                case ProcessType.Payment:
                    this.id = "payme";
                    this.title = "Payment";
                    this.processActionName = "Settled";
                    this.processType = ProcessType.Payment;
                    break;
            }

            this.CaId = caId;
            this.targetDateItems = targetDateItems;
            this.criticalDateItems = criticalDateItems;
            this.lateDateItems = lateDateItems;
            this.missingItems = missingItems;
            this.processedItemCount = processedItemCount;
            this.TimelineItems = new List<TimelineField>();
            this.totalItemCount = totalItemCount;
            this.processPercentage = this.totalItemCount != 0 ? (((decimal)this.processedItemCount) / this.totalItemCount) * 100 : 0;
        }

        public readonly string id;
        
        public readonly int CaId;

        public readonly string title;

        public readonly List<string> targetDateItems;

        public readonly List<string> criticalDateItems;

        public readonly List<string> lateDateItems;

        public readonly List<string> missingItems;

        public List<TimelineField> TimelineItems { get; set; }

        public readonly string processActionName;

        public readonly int totalItemCount;

        public readonly int processedItemCount;

        public readonly decimal processPercentage;

        public readonly ProcessType processType;
    }
}