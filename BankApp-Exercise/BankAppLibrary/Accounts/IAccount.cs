using System;

namespace BankAppLibrary.Accounts
{
    public interface IAccount
    {
        //string OwnerName { get; }
        //Guid Id { get; }
        //Guid RoutingId { get; }
        //AccountType Type { get; }
        //decimal Balance { get; }

        void Deposit(decimal amount);
        void ExternalTransfer(Guid recipientRoute, Guid recipientId, decimal amount);
        void InternalTransfer(Guid recipientId, decimal amount);
        decimal Withdraw(decimal amount, bool isTransferring = false);
    }
}