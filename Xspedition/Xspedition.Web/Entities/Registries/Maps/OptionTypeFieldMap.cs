using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Xspedition.Web.Entities.Registries.Maps
{
    public class OptionTypeFieldMap
    {
        [Key]
        [Column(Order = 0)]
        public int OptionTypeRegistryId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int FieldRegistryId { get; set; }

        [ForeignKey(nameof(OptionTypeFieldMap.OptionTypeRegistryId))]
        public virtual OptionTypeRegistry OptionType { get; set; }

        [ForeignKey(nameof(OptionTypeFieldMap.FieldRegistryId))]
        public virtual FieldRegistry Field { get; set; }
    }
}