using System;
using TenmoClient.Data;
using TenmoClient.APIClients;
using System.Collections.Generic;

namespace TenmoClient
{
    public class UserInterface
    {
        private readonly ConsoleService consoleService = new ConsoleService();
        private readonly AuthService authService = new AuthService();
        private readonly AccountAPI accountAPI = new AccountAPI();
        private readonly UserAPI userAPI = new UserAPI();
        private readonly TransferAPI transferAPI = new TransferAPI();
        private bool shouldExit = false;

        public void Start()
        {
            while (!shouldExit)
            {
                while (!authService.IsLoggedIn)
                {
                    ShowLogInMenu();
                }

                // If we got here, then the user is logged in. Go ahead and show the main menu
                ShowMainMenu();
            }
        }

        private void ShowLogInMenu()
        {
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.Write("Please choose an option: ");

            if (!int.TryParse(Console.ReadLine(), out int loginRegister))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }
            else if (loginRegister == 1)
            {
                HandleUserLogin();
            }
            else if (loginRegister == 2)
            {
                HandleUserRegister();
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void ShowMainMenu()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance"); //done
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests"); //optional
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks"); //optional
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else
                {
                    switch (menuSelection)
                    {
                        case 1:
                            GetAccountBalance();
                            break;
                        case 2:
                            GetPastTransfers(); // TODO: Implement me
                            break;
                        case 3:
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;
                        case 4:
                            GetUsers();
                            TransferFunds();
                            ShowMainMenu();
                            break;
                        case 5:
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;
                        case 6:
                            Console.WriteLine();
                            UserService.SetLogin(new API_User()); //wipe out previous login info
                            return;
                        default:
                            Console.WriteLine("Goodbye!");
                            shouldExit = true;
                            return;
                    }
                }
            }
        }

        private void HandleUserRegister()
        {
            bool isRegistered = false;

            while (!isRegistered) //will keep looping until user is registered
            {
                LoginUser registerUser = consoleService.PromptForLogin();
                isRegistered = authService.Register(registerUser);
            }

            Console.WriteLine("");
            Console.WriteLine("Registration successful. You can now log in.");
        }

        private void HandleUserLogin()
        {
            while (!UserService.IsLoggedIn) //will keep looping until user is logged in
            {
                LoginUser loginUser = consoleService.PromptForLogin();
                API_User user = authService.Login(loginUser);
                if (user != null)
                {
                    UserService.SetLogin(user);
                }
            }
        }
        private void GetAccountBalance()
        {
            decimal balance = accountAPI.GetBalance();

            Console.WriteLine($"Your account balance is ${balance}.");
            Console.WriteLine();
        }
        private void GetUsers()
        {
            Console.WriteLine("UserID  Name");
            Console.WriteLine("--------------");
            List<User> users = userAPI.GetUsers();
            foreach (User user in users)
            {
                Console.WriteLine(user.UserId.ToString() + ") ".PadRight(3) + user.UserName);
            }
        }

        private void TransferFunds()
        {
            bool done = false;
            while (!done)
            { 

                Transfer transfer = new Transfer();

                TransferRequest tRequest = new TransferRequest();
                Console.WriteLine("Enter ID of user you are sending to(0 to cancel): ");
                tRequest.ToUserID = int.Parse(Console.ReadLine());

                if(tRequest.ToUserID == 0)
                {
                    ShowMainMenu();
                }

                Console.WriteLine();
                Console.WriteLine("Enter amount: ");
                tRequest.Amount = decimal.Parse(Console.ReadLine());
                Console.WriteLine();


                if (tRequest.Amount > accountAPI.GetBalance())
                {
                    Console.WriteLine("Insufficient funds.");
                    Console.WriteLine();
                    return;
                }

                bool fundsTransferred = transferAPI.TransferFunds(tRequest);
                if (fundsTransferred)
                {
                    Console.WriteLine("Your transfer has been completed.");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Unable to transfer funds.");
                    Console.WriteLine();
                }
            }
        }
        public int GetToUserId()
        {
            bool done = false;
            int result = 0;
            while (!done)
            {
                try
                {
                    Console.WriteLine("Enter ID of user you are sending to(0 to cancel): ");
                    result = int.Parse(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to transfer funds: {ex.Message}");
                    Console.WriteLine();
                }
            }
            return result;
        }
        public void GetPastTransfers()
        {
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("TransfersID          From/To             Amount");
            Console.WriteLine("------------------------------------------------");
            List<TransferResponse> transfers = transferAPI.GetPastTransfers();
            foreach(TransferResponse transfer in transfers)
            {
                Console.WriteLine(transfer.TransferId + " " + "FROM: " + transfer.FromName + " " + "TO: " + transfer.ToName + " " + transfer.Amount);
            }

            Console.WriteLine("Please enter transfer ID to view details (0 to cancel)");
            int transferNumber = int.Parse(Console.ReadLine());

            if (transferNumber == 0)
            {
                ShowMainMenu();
            }


        }
    }
}
