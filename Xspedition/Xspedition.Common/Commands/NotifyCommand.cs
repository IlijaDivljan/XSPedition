using System.Collections.Generic;
using Xspedition.Common.Dto;

namespace Xspedition.Common.Commands
{
    public class NotifyCommand: Command
    {
        public List<NotificationDto> Notifications { get; set; }

        public NotifyCommand() : base(CommandType.Notify)
        {
            this.Notifications = new List<NotificationDto>();
        }
    }
}
