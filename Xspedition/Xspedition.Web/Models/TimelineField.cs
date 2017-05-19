using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xspedition.Common;

namespace Xspedition.Web.Models
{
    public class TimelineField
    {
        public string Content { get; set; }

        public DateTime Date { get; set; }

        public ProcessedDateCategory ProcessedDateCategory { get; set; }
    }
}