using System;

namespace BankAppLibrary
{
    public class CheckingAcc : Account
    {
        public CheckingAcc(string ownerName, Guid id, IBank bank) : base(ownerName, id, AccountType.Checking, bank)
        {
        }
    }
}
