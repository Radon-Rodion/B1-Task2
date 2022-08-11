namespace Task2.Models
{
    public class TotalInfo
    {
        public string Name { get; set; }
        public DateTime PeriodFrom { get; set; }
        public int PeriodTo { get; set; }
        public double IncomingActive { get; set; }
        public double IncomingPassive { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public double OutgoingActive { get; set; }
        public double OutgoingPassive { get; set; }
        public OperationsClassInfo[] OperationClassInfos { get; set; }
    }

    public class OperationsClassInfo
    {
        public string Name { get; set; }
        public double IncomingActive { get; set; }
        public double IncomingPassive { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public double OutgoingActive { get; set; }
        public double OutgoingPassive { get; set; }
        public AccountGroupInfo[] AccountGroupInfos { get; set; }
    }

    public class AccountGroupInfo
    {
        public int Id { get; set; }
        public double IncomingActive { get; set; }
        public double IncomingPassive { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public double OutgoingActive { get; set; }
        public double OutgoingPassive { get; set; }
        public AccountInfo[] AccountInfos { get; set; }
    }

    public class AccountInfo
    {
        public int FullId { get; set; }
        public double IncomingActive { get; set; }
        public double IncomingPassive { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public double OutgoingActive { get; set; }
        public double OutgoingPassive { get; set; }
    }
}
