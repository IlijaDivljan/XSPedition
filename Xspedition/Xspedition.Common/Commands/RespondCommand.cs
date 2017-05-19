using System.Collections.Generic;
using Xspedition.Common.Dto;

namespace Xspedition.Common.Commands
{
    public class RespondCommand: Command
    {
        public List<ResponseDto> Responses { get; set; }

        public RespondCommand(): base(CommandType.Respond)
        {
            Responses = new List<ResponseDto>();
        }
    }
}
