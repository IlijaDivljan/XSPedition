using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Xspedition.Common;

namespace Xspedition.Web.Entities
{
    public class ResponseInfo
    {
        [Key]
        public int ResponseInfoId { get; set; }

        public int CaId { get; set; }

        public int? CaTypeId { get; set; }

        public string VolManCho { get; set; }

        public int? OptionNumber { get; set; }

        public string AccountNumber { get; set; }

        public string FieldDisplay { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public ProcessedDateCategory ProcessedDateCategory { get; set; }

        public bool IsSubmitted { get; set; }
    }
}