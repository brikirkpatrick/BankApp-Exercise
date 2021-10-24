using System;

namespace BankAppLibrary.Accounts
{
    public enum InvestmentAccType
    {
        Individual,
        Corporate,
        Unsupported
    }

    public class InvestmentAccount : Account
    {
        public InvestmentAccType InvestmentType{ get; }

        public InvestmentAccount(string ownerName, Guid id, InvestmentAccType investmentType) : base(ownerName, id, AccountType.Investment)
        {
            InvestmentType = investmentType;
        }
    }
}
