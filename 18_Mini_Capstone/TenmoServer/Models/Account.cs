using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }

        public Account(int account_Id, int user_Id, decimal balance)
        {
            AccountId = account_Id;
            UserId = user_Id;
            Balance = balance;
        }
        public Account()
        {

        }
    }
}
