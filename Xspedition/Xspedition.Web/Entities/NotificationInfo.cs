using System;
using System.ComponentModel.DataAnnotations;
using Xspedition.Common;

namespace Xspedition.Web.Entities
{
    public class NotificationInfo
    {
        [Key]
        public int NotificationInfoId { get; set; }

        public int CaId { get; set; }

        public int? CaTypeId { get; set; }

        public string VolManCho { get; set; }

        public string FieldDisplay { get; set; }

        public string AccountNumber { get; set; }

        public string Recipient { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public ProcessedDateCategory ProcessedDateCategory { get; set; }

        public bool IsSent { get; set; }
    }
}