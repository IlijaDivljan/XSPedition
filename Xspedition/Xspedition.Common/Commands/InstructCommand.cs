using System.Collections.Generic;
using Xspedition.Common.Dto;

namespace Xspedition.Common.Commands
{
    public class InstructCommand: Command
    {
        public List<InstructionDto> Instructions { get; set; }

        public InstructCommand() : base(CommandType.Instruct)
        {
            this.Instructions = new List<InstructionDto>();
        }
    }
}
