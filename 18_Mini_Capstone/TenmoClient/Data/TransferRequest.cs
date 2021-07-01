using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class TransferRequest
    {
        public int ToUserID { get; set; }
        public decimal Amount { get; set; }
    }
}
