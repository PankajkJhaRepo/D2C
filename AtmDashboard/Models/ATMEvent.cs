using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtmDashboard.Models
{
    public class ATMEvent
    {
        public string CardNumber { get; set; }
        public string ATM1 { get; set; }
        public string ATM2 { get; set; }
        public string TransactionTime1 { get; set; }
        public string TransactionTime2 { get; set; }
    }
}