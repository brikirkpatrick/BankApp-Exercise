using BankAppLibrary.Accounts;
using System;

namespace BankAppLibrary
{
    public class CorporateInvestmentAcc : InvestmentAccount
    {
        public CorporateInvestmentAcc(string ownerName, Guid id) : base(ownerName, id, InvestmentAccType.Corporate)
        {
        }
    }
}
