using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xspedition.Web.ViewModels
{
    public class SummaryCaModel
    {
        public SummaryCaModel()
        {
            ProcessesPrecentage = new List<ProcessPrecentageModel>();
        }
        public int CaId { get; set; }
        public int CaTypeId { get; set; }
        public List<ProcessPrecentageModel> ProcessesPrecentage { get; set; }
    }
}