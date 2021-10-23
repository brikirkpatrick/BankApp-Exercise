using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppLibrary
{
    public enum AccountType
    {
        Checking,
        IndividualInvestment,
        CorportateInvestment,
        Unsupported
    }

    public abstract class Account
    {
        public string OwnerName { get; }
        public float Balance { get; protected set; }
        public Guid Id { get; }
        public AccountType Type { get; }

        // TODO: null checks and length checks
        public Account(string ownerName, Guid id, AccountType type)
        {
            OwnerName = ownerName;
            Id = id;
            Type = type;
            Balance = 0;
        }

        public void Deposit(int amount) 
        {
            Balance = Balance - amount;
        }

        public void Transfer(int amount) 
        {

        }

        abstract public float Withdraw(int amount);
    }
}
