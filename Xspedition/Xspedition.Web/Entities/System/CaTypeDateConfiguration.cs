using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Xspedition.Web.Entities.Shared;

namespace Xspedition.Web.Entities.System
{
    public class CaTypeDateConfiguration
    {
        [Key]
        public int DateConfigurationId { get; set; }

        public int CaType { get; set; }

        public ProcessType ProcessType { get; set; }

        public int FieldRegistryId { get; set; }

        public int DateOffset { get; set; }

        public string DateType { get; set; }
    }
}