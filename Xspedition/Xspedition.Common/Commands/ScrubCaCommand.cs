using System.Collections.Generic;
using Xspedition.Common.Dto;

namespace Xspedition.Common.Commands
{
    public class ScrubCaCommand : Command
    {
        public Dictionary<int, string> Fields { get; set; }

        public string FieldsContent { get; set; }

        public List<OptionDto> Options { get; set; }

        public ScrubCaCommand() : base(CommandType.Scrub)
        {
            this.Fields = new Dictionary<int, string>();
            this.Options = new List<OptionDto>();
        }
    }
}
