using System;

namespace BankAppLibrary.Accounts
{
    public enum InvestmentAccType
    {
        Individual,
        Corporate,
        Unsupported
    }

    public abstract class InvestmentAccount : Account
    {
        public InvestmentAccType InvestmentType{ get; }

        public InvestmentAccount(string ownerName, Guid id, Bank bank, InvestmentAccType investmentType) : base(ownerName, id, AccountType.Investment, bank)
        {
            InvestmentType = investmentType;
        }
    }
}
