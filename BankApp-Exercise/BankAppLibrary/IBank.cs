using BankAppLibrary.Accounts;
using System;
using System.Collections.Generic;

namespace BankAppLibrary
{
    public interface IBank
    {
        string Name { get; }
        Guid RoutingId { get; }
        Dictionary<Guid, Account> Accounts { get; }
        IRoutingSystem RoutingSystem { get; set; }

        Account GetAccountById(Guid accId);
        CheckingAcc RegisterNewCheckingAccount(string ownerName);
        InvestmentAcc RegisterNewInvestmentAccount(string ownerName, InvestmentAccType invType);
    }
}