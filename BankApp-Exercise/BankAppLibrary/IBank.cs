using BankAppLibrary.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppLibrary
{
    interface IBank
    {
        public CheckingAcc AddNewCheckingAccount(string ownerName);
        public InvestmentAccount AddNewInvestmentAccount(string ownerName, InvestmentAccType invType);
        public bool DepositToAccount(Guid accId, float amount);
        public bool WithdrawFromAccount(Guid accId, float amount);
        // TODO: different ways to handle transfer. Think of a better name?
        public bool TransferFromAccToAcc(Guid senderId, Guid receiverId, Bank bank, float amount);
    }
}
