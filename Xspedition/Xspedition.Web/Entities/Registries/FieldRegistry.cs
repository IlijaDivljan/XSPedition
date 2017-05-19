using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Xspedition.Web.Entities.Registries
{
    public class FieldRegistry
    {
        [Key]
        public int FieldRegistryId { get; set; }

        [Required]
        public string FieldDisplay { get; set; }

        [Required]
        public string FieldType { get; set; }
    }
}