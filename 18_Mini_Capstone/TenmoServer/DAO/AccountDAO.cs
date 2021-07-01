using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountDAO : IAccountDAO
    {
        private string connectionString;
        private string sqlGetAccountBalance =
            "SELECT balance, account_id, user_id FROM accounts WHERE user_id = @user_id";

        public AccountDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Account GetAccount(int user_Id)
        {
            Account account = new Account();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlGetAccountBalance, conn);
                    cmd.Parameters.AddWithValue("@user_id", user_Id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        account.Balance = Convert.ToDecimal(reader["balance"]);
                        account.AccountId = Convert.ToInt32(reader["account_id"]);
                        account.UserId = Convert.ToInt32(reader["user_id"]);
                    }
                }
            }
            catch (Exception ex)
            {
                account = new Account();
            }
            return account;
        }
    }
}
