using System;

namespace BankAppLibrary.Accounts
{
    public interface IAccount
    {
        void Deposit(decimal amount);
        void ExternalTransfer(Guid recipientRoute, Guid recipientId, decimal amount);
        void InternalTransfer(Guid recipientId, decimal amount);
        decimal Withdraw(decimal amount, bool isTransferring = false);
    }
}