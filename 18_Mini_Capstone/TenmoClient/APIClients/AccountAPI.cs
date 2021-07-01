using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using TenmoClient.Data;

namespace TenmoClient.APIClients
{
    public class AccountAPI : AuthService
    {
        private readonly string API_URL = "https://localhost:44315/account/";

        public decimal GetBalance()
        {
            RestRequest request = new RestRequest(API_URL);
            IRestResponse<Account> response = client.Get<Account>(request);
            if(response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            else
            {
                return response.Data.Balance;
            }
        }
    }
}
