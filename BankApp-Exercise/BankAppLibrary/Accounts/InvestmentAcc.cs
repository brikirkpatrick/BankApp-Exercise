using System;

namespace BankAppLibrary.Accounts
{
    public enum InvestmentAccType
    {
        Individual,
        Corporate,
        Unsupported
    }

    public abstract class InvestmentAcc : Account
    {
        public InvestmentAccType InvestmentType{ get; }

        public InvestmentAcc(string ownerName, Guid id, IBank bank, InvestmentAccType investmentType) : base(ownerName, id, AccountType.Investment, bank)
        {
            InvestmentType = investmentType;
        }
    }
}
