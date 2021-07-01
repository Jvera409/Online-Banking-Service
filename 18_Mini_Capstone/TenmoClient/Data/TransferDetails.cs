using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class TransferDetails
    {
        public string ToName { get; set; }
        public string FromName { get; set; }
        public decimal Amount { get; set; }
        public int TransferId { get; set; }
        public string TransferType { get; set; }
        public string TransferStatus { get; set; }
    }
}
