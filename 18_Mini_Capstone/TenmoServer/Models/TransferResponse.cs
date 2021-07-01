using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class TransferResponse
    {
        public int TransferId { get; set; }
        public int AccountToId { get; set; }
        public int AccountFromId { get; set; }
        public string ToName { get; set; }
        public string FromName { get; set; }
        public decimal Amount { get; set; }


    }
}
