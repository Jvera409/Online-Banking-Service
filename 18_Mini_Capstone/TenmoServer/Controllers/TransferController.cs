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

    public class TransferController : Controller
    {
        private ITransferDAO transferDAO;
        private IAccountDAO accountDAO;

        public TransferController(ITransferDAO transferDAO, IAccountDAO accountDAO)
        {
            this.transferDAO = transferDAO;
            this.accountDAO = accountDAO;
        }

        [HttpPost]
        public ActionResult<bool> TransferFunds(TransferRequest transferReq)
        {
            //Fleshing out our Transfer object here
            Transfer transfer = new Transfer();
            transfer.TypeId = 1001;
            transfer.StatusId = 2001;
            transfer.Amount = transferReq.Amount;

            Account toAccount = accountDAO.GetAccount(transferReq.ToUserID);
            transfer.AccountToId = toAccount.AccountId;

            int fromUserId = int.Parse(this.User.FindFirst("sub").Value);
            Account fromAccount = accountDAO.GetAccount(fromUserId);
            transfer.AccountFromId = fromAccount.AccountId;


            //Passing our Transfer object into a monster sql method from TransferDAO
            //monster sql method: inserts transfer & updates balance on both to/from accounts
            bool result = transferDAO.TransferFunds(transfer);

            if (result)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpGet]
        public ActionResult<List<Transfer>> GetTransfers()
        {

            string fromName = User.Identity.Name;
            List<TransferResponse> transfers = transferDAO.GetPastTransfers(fromName);

            
            return Ok(transfers);
            
        }

        [HttpGet]
        public ActionResult<TransferDetails> GetTransferDetails(int transferNumber)
        {

            string fromName = User.Identity.Name;
            TransferDetails transfers = transferDAO.GetTransferDetails(fromName, transferNumber);


            return Ok(transfers);

        }

    }
}
