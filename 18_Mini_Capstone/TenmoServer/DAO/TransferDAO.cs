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

        private string sqlGetPastTransfers = "SELECT transfer_id, account_from, account_to, amount, username FROM users " +
            " JOIN accounts ON users.user_id = accounts.user_id " +
            " JOIN transfers ON accounts.account_id = transfers.account_to";

        private string sqlGetTransferDetails = "SELECT transfer_id, account_from, account_to, amount, transfer_status_desc, transfer_type_desc, username FROM transfers " +
            " JOIN transfer_statuses ON transfers.transfer_status_id = transfer_statuses.transfer_status_id " +
            " JOIN transfer_types ON transfers.transfer_type_id = transfer_types.transfer_type_id " +
            " JOIN accounts ON transfers.account_to = accounts.account_id " +
            " JOIN users ON accounts.user_id = users.user_id WHERE transfer_id = @transfer_id";


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

                    cmdUpdateToAcc.Parameters.AddWithValue("@amount", transfer.Amount);
                    cmdUpdateToAcc.Parameters.AddWithValue("@account_to", transfer.AccountToId);

                    int count3 = cmdUpdateToAcc.ExecuteNonQuery();

                    //need to ask John if this is the right way to handle return for this method
                    if (count1 > 0 && count2 > 0 && count3 > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        public List<TransferResponse> GetPastTransfers(string fromName)
        {
            List<TransferResponse> transfers = new List<TransferResponse>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlGetPastTransfers, conn);
                    //cmd.Parameters.AddWithValue("@user_id", user_Id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read() == true)
                    {
                        TransferResponse transfer = new TransferResponse();
                        transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
                        transfer.AccountFromId = Convert.ToInt32(reader["account_from"]);
                        transfer.AccountToId = Convert.ToInt32(reader["account_to"]);
                        transfer.ToName = Convert.ToString(reader["username"]);
                        transfer.FromName = fromName;
                        transfer.Amount = Convert.ToDecimal(reader["amount"]);

                        transfers.Add(transfer);
                    }
                }
            }
            catch (Exception ex)
            {
                transfers = new List<TransferResponse>();
            }
            return transfers;
        }
        public TransferDetails GetTransferDetails(string fromName, int transferId)
        {
            TransferDetails transfer = new TransferDetails();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlGetTransferDetails, conn);
                    cmd.Parameters.AddWithValue("@transfer_id", transferId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read() == true)
                    {
                        transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
                        transfer.ToName = Convert.ToString(reader["username"]);
                        transfer.FromName = fromName;
                        transfer.Amount = Convert.ToDecimal(reader["amount"]);
                        transfer.TransferStatus = Convert.ToString(reader["transfer_status_desc"]);
                        transfer.TransferType = Convert.ToString(reader["transfer_type_desc"]);

                    }
                }
            }
            catch (Exception ex)
            {
                transfer = new TransferDetails();
            }
            return transfer;
        }
    }

}
