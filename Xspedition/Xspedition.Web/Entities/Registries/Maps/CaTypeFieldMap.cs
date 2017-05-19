using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Xspedition.Web.Entities.Registries.Maps
{
    public class CaTypeFieldMap
    {
        [Key]
        [Column(Order = 0)]
        public int CaTypeRegistryId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int FieldRegistryId { get; set; }

        [ForeignKey(nameof(CaTypeFieldMap.CaTypeRegistryId))]
        public virtual CaTypeRegistry CaType { get; set; }

        [ForeignKey(nameof(CaTypeFieldMap.FieldRegistryId))]
        public virtual FieldRegistry Field { get; set; }
    }
}