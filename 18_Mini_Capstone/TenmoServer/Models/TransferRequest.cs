using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoServer.Models
{
    public class TransferRequest
    {
        public int ToUserID { get; set; }
        public decimal Amount { get; set; }
    }
}
