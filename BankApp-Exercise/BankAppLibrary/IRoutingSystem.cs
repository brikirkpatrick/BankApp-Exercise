using BankAppLibrary.Accounts;
using System;

namespace BankAppLibrary
{
    public interface IRoutingSystem
    {
        public void RegisterBank(IBank bank);
        public void DeregisterBank(IBank bank);
        public void ExternalTransfer(Account sender, Guid recipientRoute, Guid recipientId, decimal amount);
    }
}
