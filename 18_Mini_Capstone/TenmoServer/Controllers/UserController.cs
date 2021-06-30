using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IUserDAO userDAO;

        public UserController(IUserDAO userDAO)
        {
            this.userDAO = userDAO;
        }

        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            int user_id = int.Parse(this.User.FindFirst("sub").Value);

            List<User> users = userDAO.GetUsers();
            List<User> usersFinal = new List<User>();

            foreach (User user in users)
            {
                if(user.UserId != user_id)
                {
                    usersFinal.Add(user);
                }
            }
                return Ok(usersFinal);
        }
    }
}
