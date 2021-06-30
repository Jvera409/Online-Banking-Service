using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class TransferController : Controller
    {
        private ITransferDAO transferDAO;

        public TransferController(ITransferDAO transferDAO)
        {
            this.transferDAO = transferDAO;
        }

        [HttpPost]
    }
}
