using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xspedition.Common.Commands;
using Xspedition.Common.Dto;

namespace Xspedition.Loader
{
    abstract public class BaseProcessParser
    {
        public Command Command { get; private set; }

        public BaseProcessParser(Command command)
        {
            Command = command;
        }

        public Command ParseContent(string[] content)
        {
            // 0, 5 -> Scrubbing,1/1/2017,N/A,1,1,M

            Command.EventDate = Convert.ToDateTime(content[1]);
            Command.CaId = int.Parse(content[3]);
            Command.CaTypeId = int.Parse(content[4]);
            Command.VolManCho = content[5];

            ParseCustomContent(content);

            return Command;
        }

        protected abstract void ParseCustomContent(string[] content);
    }

    public class ScrubbingParser : BaseProcessParser
    {
        public static ScrubbingParser CreateParser()
        {
            return new ScrubbingParser();
        }

        public ScrubbingParser()
            : base(new ScrubCaCommand()) { }

        protected override void ParseCustomContent(string[] content)
        {
            ParseCAFields(content[7]);

            ParseOptionFields(content, 8);

            ParseOptionFields(content, 17);

            ParseOptionFields(content, 26);
        }


        private void ParseCAFields(string content)
        {
            // 7 = 1;2|01/01/2017;3;4,1,1,1;2|5/05/2017;3;4,
            if (string.IsNullOrWhiteSpace(content))
                return;

            ((ScrubCaCommand)Command).FieldsContent = content.Trim();
        }

        private void ParseOptionFields(string[] content, int index)
        {
            // 8 - 10 1,1;2;3;4
            // 17 - 19
            if (string.IsNullOrWhiteSpace(content[index]))
                return;

            OptionDto option = new OptionDto();
            option.OptionNumber = int.Parse(content[index]);
            option.OptionTypeId = string.IsNullOrWhiteSpace(content[index + 1]) ? null : (int?)int.Parse(content[index + 1]);

            option.FieldsContent = content[index + 2].Trim();

            ParsePayoutFields(content, index + 3, option);
            ParsePayoutFields(content, index + 6, option);

            ((ScrubCaCommand)Command).Options.Add(option);
        }

        private void ParsePayoutFields(string[] content, int index, OptionDto option)
        {
            // 2,1;2;3;4,3,3
            // 11 - 13, 20 - 22
            // 14 - 16 , 23 - 25
            if (string.IsNullOrWhiteSpace(content[index]))
                return;

            PayoutDto payout = new PayoutDto();
            payout.PayoutNumber = int.Parse(content[index]);
            payout.PayoutTypeId = string.IsNullOrWhiteSpace(content[index + 1]) ? null : (int?)int.Parse(content[index + 1]);

            payout.FieldsContent = content[index + 2].Trim();

            option.Payouts.Add(payout);
        }
    }

    public class NotificationParser : BaseProcessParser
    {
        public static NotificationParser CreateParser()
        {
            return new NotificationParser();
        }

        public NotificationParser()
            : base(new NotifyCommand()) { }
        
        protected override void ParseCustomContent(string[] content)
        {
            //6 = ,20122557|MICHAEL J BRUNELL; GU020446|CHRIS HANDLEY,
            string[] notifications = content[6].Trim().Split(';');
            foreach (string notification in notifications)
            {
                if (string.IsNullOrWhiteSpace(notification))
                    continue;

                string[] pair = notification.Trim().Split('|');
                ((NotifyCommand)Command).Notifications.Add(new NotificationDto { AccountNumber = pair[0], Recipient = pair[1], IsSent = Convert.ToBoolean(content[2]) });
            }
        }
    }

    public class ResponseParser : BaseProcessParser
    {
        public static ResponseParser CreateParser()
        {
            return new ResponseParser();
        }

        public ResponseParser()
            : base(new RespondCommand()) { }
        protected override void ParseCustomContent(string[] content)
        {
            //6 = ,20122557|MICHAEL J BRUNELL; GU020446|CHRIS HANDLEY,
            string[] responses = content[6].Trim().Split(';');
            foreach (string response in responses)
            {
                if (string.IsNullOrWhiteSpace(response))
                    continue;

                string[] pair = response.Trim().Split('|');
                ((RespondCommand)Command).Responses.Add(new ResponseDto { AccountNumber = pair[0], OptionNumber = pair.Length > 1 ? (int?)int.Parse(pair[1]) : null, IsSubmitted = Convert.ToBoolean(content[2]) });
            }
        }
    }

    public class InstructionParser : BaseProcessParser
    {
        public static InstructionParser CreateParser()
        {
            return new InstructionParser();
        }

        public InstructionParser()
            : base(new InstructCommand()) { }
        protected override void ParseCustomContent(string[] content)
        {
            //6 = ,20122557;GU020446,
            string[] instructions = content[6].Trim().Split(';');
            foreach (string instruction in instructions)
            {
                if (string.IsNullOrWhiteSpace(instruction))
                    continue;
                
                ((InstructCommand)Command).Instructions.Add(new InstructionDto { AccountNumber = instruction, IsInstructed = Convert.ToBoolean(content[2]) });
            }
        }
    }

    public class PaymentParser : BaseProcessParser
    {
        public static PaymentParser CreateParser()
        {
            return new PaymentParser();
        }

        public PaymentParser()
            : base(new PayCommand()) { }
        protected override void ParseCustomContent(string[] content)
        {
            //6 = ,GU020446|2|1;GU020446|2|2,
            string[] responses = content[6].Trim().Split(';');
            foreach (string response in responses)
            {
                if (string.IsNullOrWhiteSpace(response))
                    continue;

                string[] pair = response.Trim().Split('|');
                ((PayCommand)Command).Payments.Add(new PaymentDto { AccountNumber = pair[0], OptionNumber = int.Parse(pair[1]), PayoutNumber = int.Parse(pair[2]), IsSettled = Convert.ToBoolean(content[2]) });
            }
        }
    }
}
