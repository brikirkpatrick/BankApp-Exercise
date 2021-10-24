using System;

namespace BankAppLibrary
{
    public class CorporateInvestmentAcc : Account
    {
        public CorporateInvestmentAcc(string ownerName, Guid id) : base(ownerName, id, AccountType.CorportateInvestment)
        {
        }
    }
}
