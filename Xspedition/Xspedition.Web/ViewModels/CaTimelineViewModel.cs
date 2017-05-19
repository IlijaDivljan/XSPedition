using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xspedition.Web.Entities;
using Xspedition.Web.Models;

namespace Xspedition.Web.ViewModels
{
    public class CaTimelineViewModel
    {
        public int CaId { get; set; }

        public CaTimeline TimelineRegions { get; set; }

        public List<TimelineField> TimelineDateFields { get; set; }

        public List<CaProcessViewModel> CaProcessViewModels { get; set; }
    }
}