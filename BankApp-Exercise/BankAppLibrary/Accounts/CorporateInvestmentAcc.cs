using System;

namespace BankAppLibrary.Accounts
{
    public class CorporateInvestmentAcc : InvestmentAcc
    {
        public CorporateInvestmentAcc(string ownerName, Guid id, IBank bank) : base(ownerName, id, bank, InvestmentAccType.Corporate)
        {
        }
    }
}
