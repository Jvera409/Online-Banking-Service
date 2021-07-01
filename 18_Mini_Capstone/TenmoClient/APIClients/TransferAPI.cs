using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient.APIClients
{

    public class TransferAPI : AuthService
    {
        private readonly string API_URL = "https://localhost:44315/transfer/";

        public bool TransferFunds(TransferRequest tRequest)
        {
            RestRequest request = new RestRequest(API_URL);
            request.AddJsonBody(tRequest);
            IRestResponse<bool> response = client.Post<bool>(request);
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
        public List<TransferResponse> GetPastTransfers()
        {
            RestRequest request = new RestRequest(API_URL);
            IRestResponse<List<TransferResponse>> response = client.Get<List<TransferResponse>>(request);
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
            //can make separate class/data type to pass information from client to server
        }
    }
}
