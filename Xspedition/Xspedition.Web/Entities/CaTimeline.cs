using System;
using System.ComponentModel.DataAnnotations;

namespace Xspedition.Web.Entities
{
    public class CaTimeline
    {
        [Key]
        public int CaTimelineId { get; set; }

        public int CaId { get; set; }

        public DateTime? ScrubbingStart { get; set; }
                       
        public DateTime? ScrubbingTarget { get; set; }
                       
        public DateTime? ScrubbingCritical { get; set; }
                       
        public DateTime? NotificationStart { get; set; }
                       
        public DateTime? NotificationTarget { get; set; }
                       
        public DateTime? NotificationCritical { get; set; }
                       
        public DateTime? ResponseStart { get; set; }
                       
        public DateTime? ResponseTarget { get; set; }
                       
        public DateTime? ResponseCritical { get; set; }
                       
        public DateTime? InstructionStart { get; set; }
                       
        public DateTime? InstructionTarget { get; set; }
                       
        public DateTime? InstructionCritical { get; set; }
                       
        public DateTime? PaymentStart { get; set; }
                       
        public DateTime? PaymentTarget { get; set; }
                       
        public DateTime? PaymentCritical { get; set; }
    }
}