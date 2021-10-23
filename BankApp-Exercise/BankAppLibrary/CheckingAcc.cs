using System;

namespace BankAppLibrary
{
    public class CheckingAcc : Account
    {
        public CheckingAcc(string ownerName, Guid id) : base(ownerName, id, AccountType.Checking)
        {
        }

        public override float Withdraw(int amount)
        {
            throw new NotImplementedException();
        }
    }
}
