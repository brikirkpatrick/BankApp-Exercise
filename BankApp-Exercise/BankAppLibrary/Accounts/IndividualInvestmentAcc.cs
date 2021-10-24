using System;

namespace BankAppLibrary
{
    public class IndividualInvestmentAcc : Account
    {
        public IndividualInvestmentAcc(string ownerName, Guid id) : base(ownerName, id, AccountType.IndividualInvestment)
        {
        }

        // cant withdraw over 500
        // return bool? how to verify it's correct
        // maybe throw an exception?
        public override decimal Withdraw(decimal amount)
        {
            if (amount > 500)
            {
                return 0;
            }

            Balance = Balance - amount;
            return Balance;
        }
    }
}
