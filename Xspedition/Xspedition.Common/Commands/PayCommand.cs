using System.Collections.Generic;
using Xspedition.Common.Dto;

namespace Xspedition.Common.Commands
{
    public class PayCommand: Command
    {
        public List<PaymentDto> Payments { get; set; }

        public PayCommand() : base(CommandType.Pay)
        {
            this.Payments = new List<PaymentDto>();
        }
    }
}
