﻿namespace Task2.Models
{
    public class OperationsClassBalance: IBalance
    {
        public int Id { get; set; }
        public int OperationsClassId { get; set; }
        public int LoadedListId { get; set; }
        public double IncomingActive { get; set; }
        public double IncomingPassive { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public double OutgoingActive { get; set; }
        public double OutgoingPassive { get; set; }
    }
}
