using System;

namespace BankAppLibrary
{
    public class CheckingAcc : Account
    {
        public CheckingAcc(string ownerName, Guid id, Bank bank) : base(ownerName, id, AccountType.Checking, bank)
        {
        }
    }
}
