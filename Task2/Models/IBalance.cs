namespace Task2.Models
{
    public interface IBalance
    {
        public double IncomingActive { get; set; }
        public double IncomingPassive { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public double OutgoingActive { get; set; }
        public double OutgoingPassive { get; set; }
    }
}
