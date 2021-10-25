using BankAppLibrary.Accounts;
using System;
using System.Collections.Generic;

namespace BankAppLibrary
{
    public interface IBank
    {
        Dictionary<Guid, Account> Accounts { get; }
        string Name { get; }
        Guid RoutingId { get; }
        IRoutingSystem RoutingSystem { get; set; }

        Account GetAccountById(Guid accId);
        CheckingAcc RegisterNewCheckingAccount(string ownerName);
        InvestmentAccount RegisterNewInvestmentAccount(string ownerName, InvestmentAccType invType);
    }
}