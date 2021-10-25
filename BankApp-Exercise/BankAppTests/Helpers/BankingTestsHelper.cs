using BankAppLibrary;
using BankAppLibrary.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppTests.Helpers
{
    public static class BankingTestsHelper
    {
        public static List<Guid> AddAccountOfEachTypeToBank(IBank bank, int amount = 1)
        {
            var accountIds = new List<Guid>();
            while (amount != 0)
            {
                accountIds.Add(bank.RegisterNewCheckingAccount($"CheckingOwner{amount}").Id);
                accountIds.Add(bank.RegisterNewInvestmentAccount($"IndInvestmentOwner{amount}", InvestmentAccType.Individual).Id);
                accountIds.Add(bank.RegisterNewInvestmentAccount($"CorpInvestmentOwner{amount}", InvestmentAccType.Corporate).Id);
                amount--;
            }
            return accountIds;
        }
    }
}
