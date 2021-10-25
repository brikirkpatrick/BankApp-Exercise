using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppLibrary
{
    public interface IRoutingSystem
    {
        public void ExternalTransfer(Account sender, Guid receiverRouting, Guid receiverAcc, decimal amount);
    }
}
