using System.Collections.Generic;

namespace Xspedition.Common.Dto
{
    public class PayoutDto
    {
        public int PayoutNumber { get; set; }

        public int? PayoutTypeId { get; set; }

        public Dictionary<int, string> Fields { get; set; }

        public string FieldsContent { get; set; }

        public PayoutDto()
        {
            this.Fields = new Dictionary<int, string>();
        }
    }
}