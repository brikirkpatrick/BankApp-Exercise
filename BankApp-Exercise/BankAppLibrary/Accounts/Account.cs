﻿using System;

namespace BankAppLibrary
{
    public enum AccountType
    {
        Checking,
        Investment,
        Unsupported // TODO: reconsider keeping this and Investment Unsupported. Literally only exists for testing.
    }

    // TODO: Big, how to handle decimal numbers where more than 2 decimal points? remember to adjust tests for this.
    // TODO: consider abstracts folder or project for interfaces and enums?
    // TODO: consider handling of name (like first + last) but this seems outta scope of requirements
    // TODO: Overdraft and handling/testing? may be moot
    public abstract class Account
    {
        public string OwnerName { get; }
        public Guid Id { get; }
        public AccountType Type { get; }
        public decimal Balance { get; protected set; }

        public Account(string ownerName, Guid id, AccountType type)
        {
            if (string.IsNullOrWhiteSpace(ownerName)) throw new ArgumentNullException($"{nameof(ownerName)} cannot be null or empty");
            if (id == null || id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            OwnerName = ownerName;
            Id = id;
            Type = type;
            Balance = 0;
        }

        public virtual void Deposit(decimal amount) 
        {
            if (amount < 0M) throw new ArgumentException($"{nameof(amount)} must be a positive decimal.");

            Balance = Balance + amount;
        }

        /// Withdraws a specified amount from an account
        /// Returns the amount withdrawn.
        public virtual decimal Withdraw(decimal amount)
        {
            if (amount < 0M) throw new ArgumentException($"{nameof(amount)} must be a positive decimal.");
            if (amount > Balance) throw new InvalidOperationException("amount to withdraw is greater than current account balance");

            Balance = Balance - amount;
            return amount;
        }

        public virtual void Transfer(decimal amount) 
        {

        }
    }
}
