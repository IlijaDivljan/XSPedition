using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xspedition.Web.ViewModels
{
    public class SummaryModel
    {
        public SummaryModel()
        {
            CAs = new List<SummaryCaModel>();
            SummaryProcessPrecentage = new List<ProcessPrecentageModel>();
        }
        public List<SummaryCaModel> CAs { get; set; }
        public List<ProcessPrecentageModel> SummaryProcessPrecentage { get; set; }
    }
}