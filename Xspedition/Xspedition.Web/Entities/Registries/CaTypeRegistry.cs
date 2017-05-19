using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Xspedition.Web.Entities.Registries
{
    public class CaTypeRegistry
    {
        [Key]
        public int CaTypeRegistryId { get; set; }

        [Required]
        public string Code { get; set; }
    }
}