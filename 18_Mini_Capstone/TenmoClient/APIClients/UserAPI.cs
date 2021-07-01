using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using TenmoClient.Data;

namespace TenmoClient.APIClients
{
    public class UserAPI : AuthService
    {
        private readonly string API_URL = "https://localhost:44315/user/";

        public List<User> GetUsers()
        {
            RestRequest request = new RestRequest(API_URL);
            IRestResponse<List<User>> response = client.Get<List<User>>(request);
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
