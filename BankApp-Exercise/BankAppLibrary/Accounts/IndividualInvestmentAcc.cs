using System;

namespace BankAppLibrary.Accounts
{
    public class IndividualInvestmentAcc : InvestmentAcc
    {
        public IndividualInvestmentAcc(string ownerName, Guid id, IBank bank) : base(ownerName, id, bank, InvestmentAccType.Individual)
        {
        }

        /// <summary>
        /// Withdraws a specified positive amount from an account up to $500 and returns the amount withdrawn.
        /// </summary>
        /// <param name="amount">the amount added to the balance</param>
        /// <param name="isTransferring">a parameter to override the $500 limit for transfer-withdraws</param>
        /// <returns></returns>
        public override decimal Withdraw(decimal amount, bool isTransferring = false)
        {
            if (amount > 500M && !isTransferring) throw new ArgumentOutOfRangeException(nameof(amount), "individual accounts have a withdrawal limit of $500");

            return base.Withdraw(amount, isTransferring);
        }
    }
}
