using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferDAO : ITransferDAO
    {
        private readonly string connectionString;
        private string sqlTransferFunds =
            "INSERT INTO transfers ( transfer_type_id, transfer_status_id, account_from, account_to, amount ) " +
            "VALUES ( @transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount );";
        private string sqlUpdateFromAccountBalance =
            "UPDATE accounts SET balance -= @amount WHERE account_id = @account_from;";
        private string sqlUpdateToAccountBalance =
            "UPDATE accounts SET balance += @amount WHERE account_id = @account_to;";


        public TransferDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool TransferFunds(Transfer transfer)
        {
            bool result = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    //=================================================================================
                    //Insert Transfer
                    SqlCommand cmdTransfer = new SqlCommand(sqlTransferFunds, conn);

                    cmdTransfer.Parameters.AddWithValue("@transfer_type_id", transfer.TypeId);
                    cmdTransfer.Parameters.AddWithValue("@transfer_status_id", transfer.StatusId);
                    cmdTransfer.Parameters.AddWithValue("@account_from", transfer.AccountFromId);
                    cmdTransfer.Parameters.AddWithValue("@account_to", transfer.AccountToId);
                    cmdTransfer.Parameters.AddWithValue("@amount", transfer.Amount);

                    int count1 = cmdTransfer.ExecuteNonQuery();

                    //=================================================================================
                    //Update balance of To Account
                    SqlCommand cmdUpdateFromAcc = new SqlCommand(sqlUpdateFromAccountBalance, conn);

                    cmdUpdateFromAcc.Parameters.AddWithValue("@amount", transfer.Amount);
                    cmdUpdateFromAcc.Parameters.AddWithValue("@account_from", transfer.AccountFromId);

                    int count2 = cmdUpdateFromAcc.ExecuteNonQuery();

                    //=================================================================================
                    //Update balance of From Account
                    SqlCommand cmdUpdateToAcc = new SqlCommand(sqlUpdateToAccountBalance, conn);

                    cmdUpdateFromAcc.Parameters.AddWithValue("@amount", transfer.Amount);
                    cmdUpdateFromAcc.Parameters.AddWithValue("@account_to", transfer.AccountToId);

                    int count3 = cmdUpdateToAcc.ExecuteNonQuery();

                    //need to ask John if this is the right way to handle return for this method
                    if (count1 > 0 && count2 > 0 && count3 > 0)
                    {
                        result = true;
                    }
                }
            }
            catch(Exception ex)
            {
                result = false;
            }
            return result;
        }
    }
}
