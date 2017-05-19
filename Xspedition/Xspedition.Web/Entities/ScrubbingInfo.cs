using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Xspedition.Common;

namespace Xspedition.Web.Entities
{
    public class ScrubbingInfo
    {
        [Key]
        public int ScrubbingInfoId { get; set; }

        public int FieldRegistryId { get; set; }

        public int CaId { get; set; }

        public int? CaTypeId { get; set; }

        public string VolManCho { get; set; }

        public int? OptionNumber { get; set; }

        public int? OptionTypeId { get; set; }

        public int? PayoutNumber { get; set; }

        public int? PayoutTypeId { get; set; }

        public string FieldDisplay { get; set; }

        public DateTime? DateFieldValue { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public ProcessedDateCategory ProcessedDateCategory { get; set; }

        public bool IsSrubbed { get; set; }
    }
}