using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xspedition.Web.Entities.Shared;

namespace Xspedition.Web.ViewModels
{
    public class ProcessPrecentageModel
    {
        public ProcessType ProcessType;
        public decimal Target { get; set; }
        public decimal Critical { get; set; }
        public decimal Late { get; set; }
        public decimal Missing { get; set; }
    }
}