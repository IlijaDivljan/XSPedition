using System;

namespace Xspedition.Common.Commands
{
    public abstract class Command
    {
        public int CaId { get; set; }

        public int CaTypeId { get; set; }

        public string VolManCho { get; set; }

        public DateTime EventDate { get; set; }

        public readonly CommandType Type;

        protected Command(CommandType type)
        {
            this.Type = type;
        }
    }
}
