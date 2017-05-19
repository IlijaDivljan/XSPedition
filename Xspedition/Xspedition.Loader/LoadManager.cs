using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Xspedition.Common.Commands;

namespace Xspedition.Loader
{
    public class LoadManager
    {
        const string SCRUBBING = "Scrubbing";
        const string NOTIFICATION = "Notification";
        const string RESPONSE = "Response";
        const string INSTRUCTION = "Instruction";
        const string PAYMENT = "Payment";

        private Dictionary<string, Func<BaseProcessParser>> processesParsers = new Dictionary<string, Func<BaseProcessParser>>
                                                                    {
                                                                        { SCRUBBING, ScrubbingParser.CreateParser },
                                                                        { NOTIFICATION, NotificationParser.CreateParser },
                                                                        { RESPONSE, ResponseParser.CreateParser },
                                                                        { INSTRUCTION, InstructionParser.CreateParser },
                                                                        { PAYMENT, PaymentParser.CreateParser }
                                                                    };
        private Dictionary<CommandType, Action<Command>> processesSenders = new Dictionary<CommandType, Action<Command>>
                                                                    {
                                                                        { CommandType.Scrub, DataLoader.SendScrubCommand },
                                                                        { CommandType.Notify, DataLoader.SendNotifyCommand },
                                                                        { CommandType.Respond, DataLoader.SendResponsCommand },
                                                                        { CommandType.Instruct, DataLoader.SendInstructCommand },
                                                                        { CommandType.Pay, DataLoader.SendPayCommand }
                                                                    };
        private List<Command> seedData = new List<Command>();
        private List<Command> commands = new List<Command>();
        
        public void LoadData()
        {
            seedData.Clear();

            InputReader reader = new InputReader();
            reader.ReadFromFile();

            foreach (string line in reader.Lines)
            {
                ParseContent(line);
            }

            CopySeedData();

            SendData();
        }

        private void ParseContent(string line)
        {
            string[] content = line.Split(',');
            string processKey = content[0];
            if (processesParsers.Keys.Contains(processKey))
            {
                seedData.Add(processesParsers[processKey]().ParseContent(content));
            }
        }

        private void CopySeedData()
        {
            commands.Clear();
            seedData.ForEach(c => commands.Add(c));
        }

        public void PerformeDateOffset()
        {
            int dayOffset = 5;

            seedData.ForEach(c => 
                {
                    c.CaId = seedData.Count + c.CaId;

                    c.EventDate = c.EventDate.AddDays(dayOffset);
                    if (c.Type == CommandType.Scrub)
                    {
                        ScrubCaCommand cmd = (ScrubCaCommand)c;
                        cmd.FieldsContent = AdjustDates(cmd.FieldsContent, dayOffset);
                        cmd.Options.ForEach(o =>
                        {
                            o.FieldsContent = AdjustDates(o.FieldsContent, dayOffset);
                            o.Payouts.ForEach(p => p.FieldsContent = AdjustDates(p.FieldsContent, dayOffset));
                        });
                    }
                });

            CopySeedData();

            SendData();
        }

        private string AdjustDates(string fieldsContent, int dayOffset)
        {
            if (string.IsNullOrWhiteSpace(fieldsContent))
                return fieldsContent;

            List<string> values = new List<string>();
            string[] fields = fieldsContent.Split(';');
            foreach (string field in fields)
            {
                if (string.IsNullOrWhiteSpace(field))
                    continue;

                string[] pair = field.Trim().Split('|');
                string value = pair[0];

                if (pair.Length > 1)
                {
                    DateTime date = DateTime.Parse(pair[1]).AddDays(dayOffset);
                    value += "|" + date.ToString("MM/dd/yyyy");
                }

                values.Add(value);
            }
            return string.Join(";", values);
        }

        private void SendData()
        {
            foreach (Command command in commands.OrderBy(c => c.EventDate))
            {
                processesSenders[command.Type](command);
            }
        }

        //public static void SendScrubCommand(Command command)
        //{
        //    ScrubCaCommand scrubCommand = (ScrubCaCommand)command;

        //    POST(JsonConvert.SerializeObject(scrubCommand), "SendScrubbingInputData");
        //}

        //public static void SendNotifyCommand(Command command)
        //{
        //    NotifyCommand notifyCommand = (NotifyCommand)command;

        //    POST(JsonConvert.SerializeObject(notifyCommand), "SendNotificationInputData");
        //}

        //public static void SendResponsCommand(Command command)
        //{
        //    RespondCommand responsCommand = (RespondCommand)command;

        //    POST(JsonConvert.SerializeObject(responsCommand), "SendResponseInputData");
        //}

        //private static void SendInstructCommand(Command command)
        //{
        //    InstructCommand instructCommand = (InstructCommand)command;

        //    POST(JsonConvert.SerializeObject(instructCommand), "SendInstructionInputData");
        //}

        //public static void SendPayCommand(Command command)
        //{
        //    PayCommand payCommand = (PayCommand)command;

        //    POST(JsonConvert.SerializeObject(payCommand), "SendPaymentInputData");
        //}



        //public static void POST(string serializedCommand, string target)
        //{
        //    string apiUrl = @"http://localhost:55078/Xspedition/" + target;
        //    var client = new HttpClient();
        //    var response = client.PostAsync(apiUrl, new StringContent(serializedCommand, Encoding.UTF8, "application/json")).Result;
        //}


        //public static void POST(MemoryStream memmoryStream, string target)
        //{
        //    return;

        //    //Cretae a web request where object would be sent
        //    WebRequest objWebRequest = WebRequest.Create(@"http://localhost:55078/Xspedition/" + target);
        //    objWebRequest.Method = "POST";
        //    objWebRequest.ContentLength = memmoryStream.Length;
        //    // Create a request stream which holds request data
        //    Stream reqStream = objWebRequest.GetRequestStream();
        //    //Write the memory stream data into stream object before send it.
        //    byte[] buffer = new byte[memmoryStream.Length];
        //    int count = memmoryStream.Read(buffer, 0, buffer.Length);
        //    reqStream.Write(buffer, 0, buffer.Length);

        //    //Send a request and wait for response.
        //    try
        //    {
        //        WebResponse objResponse = objWebRequest.GetResponse();
        //        //Get a stream from the response.
        //        Stream streamdata = objResponse.GetResponseStream();
        //        //read the response using streamreader class as stream is read by reader class.
        //        StreamReader strReader = new StreamReader(streamdata);
        //        string responseData = strReader.ReadToEnd();
        //    }
        //    catch (WebException ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static void POST(Command command)
        //{
        //    //Serialize the object into stream before sending it to the remote server
        //    MemoryStream memmoryStream = new MemoryStream();
        //    BinaryFormatter binayformator = new BinaryFormatter();
        //    binayformator.Serialize(memmoryStream, command);

        //    //Cretae a web request where object would be sent
        //    WebRequest objWebRequest = WebRequest.Create(@"http://localhost/XMLProvider/XMLProcessorHandler.ashx");
        //    objWebRequest.Method = "POST";
        //    objWebRequest.ContentLength = memmoryStream.Length;
        //    // Create a request stream which holds request data
        //    Stream reqStream = objWebRequest.GetRequestStream();
        //    //Write the memory stream data into stream object before send it.
        //    byte[] buffer = new byte[memmoryStream.Length];
        //    int count = memmoryStream.Read(buffer, 0, buffer.Length);
        //    reqStream.Write(buffer, 0, buffer.Length);

        //    //Send a request and wait for response.
        //    try
        //    {
        //        WebResponse objResponse = objWebRequest.GetResponse();
        //        //Get a stream from the response.
        //        Stream streamdata = objResponse.GetResponseStream();
        //        //read the response using streamreader class as stream is read by reader class.
        //        StreamReader strReader = new StreamReader(streamdata);
        //        string responseData = strReader.ReadToEnd();
        //    }
        //    catch (WebException ex)
        //    {
        //        throw ex;
        //    }

        //}
    }
}
