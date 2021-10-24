using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppLibrary
{
    interface IBank
    {
        public Account AddNewAccount(string ownerName, AccountType type);
        public bool DepositToAccount(Guid accId, float amount);
        public bool WithdrawFromAccount(Guid accId, float amount);
        // TODO: different ways to handle transfer. Think of a better name?
        public bool TransferFromAccToAcc(Guid senderId, Guid receiverId, Bank bank, float amount);
    }
}
