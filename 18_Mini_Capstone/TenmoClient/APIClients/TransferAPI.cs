using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient.APIClients
{

    class TransferAPI : AuthService
    {
        private readonly string API_URL = "https://localhost:44315/account/";

        public Transfer TranserFunds()
        {
            RestRequest request = new RestRequest(API_URL);
            IRestResponse<Transfer> response = client.Post<Transfer>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }
        }
    }
}
