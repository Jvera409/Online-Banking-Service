using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        bool TransferFunds(Transfer transfer);

        List<TransferResponse> GetPastTransfers(string fromName);
    }
}
