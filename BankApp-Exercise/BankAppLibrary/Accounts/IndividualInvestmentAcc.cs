using BankAppLibrary.Accounts;
using System;

namespace BankAppLibrary
{
    public class IndividualInvestmentAcc : InvestmentAccount
    {
        public IndividualInvestmentAcc(string ownerName, Guid id, IBank bank) : base(ownerName, id, bank, InvestmentAccType.Individual)
        {
        }

        /// Individual accounts have a withdrawal limit of 500 dollars.
        /// Note: I'm going with the assumption that a withdraw > 500 is ok if  account is transferring
        public override decimal Withdraw(decimal amount, bool isTransferring = false)
        {
            if (amount > 500M && !isTransferring) throw new ArgumentOutOfRangeException(nameof(amount), "individual accounts have a withdrawal limit of $500");

            return base.Withdraw(amount, isTransferring);
        }
    }
}
