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
    public class AccountController : Controller
    {
        private IAccountDAO accountDAO;

        public AccountController(IAccountDAO accountDAO)
        {
            this.accountDAO = accountDAO;
        }

        [HttpGet]
        public ActionResult<Account> GetBalance()
        {
            int user_id = int.Parse(this.User.FindFirst("sub").Value);

            return Ok(accountDAO.GetAccount(user_id));
        }

        //[HttpPut]
        //public ActionResult<bool> UpdateBalance()
        //{

        //}
    }
}
