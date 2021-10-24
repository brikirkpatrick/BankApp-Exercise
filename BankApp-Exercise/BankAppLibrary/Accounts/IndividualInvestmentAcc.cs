using BankAppLibrary.Accounts;
using System;

namespace BankAppLibrary
{
    public class IndividualInvestmentAcc : InvestmentAccount
    {
        public IndividualInvestmentAcc(string ownerName, Guid id) : base(ownerName, id, InvestmentAccType.Individual)
        {
        }

        /// Individual accounts have a withdrawal limit of 500 dollars.
        public override decimal Withdraw(decimal amount)
        {
            if (amount > 500M) throw new ArgumentOutOfRangeException(nameof(amount), "individual accounts have a withdrawal limit of $500");

            return base.Withdraw(amount);
        }
    }
}
