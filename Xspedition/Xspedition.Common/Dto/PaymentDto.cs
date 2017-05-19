namespace Xspedition.Common.Dto
{
    public class PaymentDto
    {
        public string AccountNumber { get; set; }

        public int OptionNumber { get; set; }

        public int PayoutNumber { get; set; }

        public bool IsSettled { get; set; }
    }
}