using System.Collections.Generic;

namespace Xspedition.Common.Dto
{
    public class OptionDto
    {
        public int OptionNumber { get; set; }

        public int? OptionTypeId { get; set; }

        public Dictionary<int, string> Fields { get; set; }

        public string FieldsContent { get; set; }

        public List<PayoutDto> Payouts { get; set; }

        public OptionDto()
        {
            this.Fields = new Dictionary<int, string>();
            this.Payouts = new List<PayoutDto>();
        }
    }
}