using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        public override string ToString()
        {
            return $"{UserId}) {UserName}";
        }

    }
}
