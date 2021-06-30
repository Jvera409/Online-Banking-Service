using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    class Transfer
    {
        public int TransferId { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }
    }
}
