using System;

namespace BankAppLibrary.Accounts
{
    public enum AccountType
    {
        Checking,
        Investment
    }

    // TODO: Make note of passed considerations in ReadMe
    // - Overdraft
    // - OwnerName + BankName sanitization
    // - RoutingSystem using property injection
    // - Bank as a param for Account constructor (why'd I do it?)
    // - I'm going with the assumption that a withdraw > 500 is ok if  account is transferring
    // - Are the Guids I'm using unique enough? Should I improve on uniqueness?

    public abstract class Account : IAccount
    {
        private IBank _bank;
        public string OwnerName { get; }
        public Guid Id { get; }
        public Guid RoutingId { get; }
        public AccountType Type { get; }
        public decimal Balance { get; protected set; }

        public Account(string ownerName, Guid id, AccountType type, IBank bank)
        {
            if (string.IsNullOrWhiteSpace(ownerName)) throw new ArgumentNullException(nameof(ownerName), "cannot be null or empty");
            if (id == null || id == Guid.Empty) throw new ArgumentNullException(nameof(id), "cannot be null or empty");
            if (bank == null) throw new ArgumentNullException(nameof(bank), "cannot be null");
            if (bank.RoutingId == null) throw new ArgumentNullException(nameof(bank.RoutingId), "cannot be null");

            _bank = bank;
            RoutingId = bank.RoutingId;
            OwnerName = ownerName;
            Id = id;
            Type = type;
            Balance = 0;
        }

        /// <summary>
        ///  Deposits an amount to an account
        /// </summary>
        /// <param name="amount">the amount added to the balance</param>
        public virtual void Deposit(decimal amount)
        {
            if (amount <= 0M) throw new ArgumentOutOfRangeException($"{nameof(amount)} must be a positive decimal");

            Balance = Balance + amount;
        }

        /// <summary>
        ///  Withdraws a specified amount from an account and returns the amount withdrawn.
        /// </summary>
        /// <param name="amount">the amount added to the balance</param>
        /// <param name="isTransferring">an optional parameter for handling transfer-withdraw cases</param>
        /// <returns></returns>
        public virtual decimal Withdraw(decimal amount, bool isTransferring = false)
        {
            if (amount <= 0M) throw new ArgumentOutOfRangeException(nameof(amount), "amount must be a positive decimal");
            if (amount > Balance) throw new ArgumentOutOfRangeException(nameof(amount), "amount to withdraw is greater than current account balance");

            Balance = Balance - amount;
            return amount;
        }

        /// <summary>
        ///  Transfers an amount to an internal account.
        /// </summary>
        /// <param name="recipientId">recipient account id</param>
        /// <param name="amount">the amount transferred</param>
        public virtual void InternalTransfer(Guid recipientId, decimal amount)
        {
            var receiver = _bank.GetAccountById(recipientId);
            decimal withdrawAmt = this.Withdraw(amount, true);
            receiver.Deposit(withdrawAmt);
        }

        /// <summary>
        ///  Transfers an amount to an external account.
        /// </summary>
        /// <param name="recipientRoute">recipient routing id</param>
        /// <param name="recipientId">recipient account id</param>
        /// <param name="amount">the amount transferred</param>
        public virtual void ExternalTransfer(Guid recipientRoute, Guid recipientId, decimal amount)
        {
            if (_bank.RoutingSystem == null)
            {
                throw new NullReferenceException("the bank associated with this account is not registered to a routingSystem");
            }

            _bank.RoutingSystem.ExternalTransfer(this, recipientRoute, recipientId, amount);
        }
    }
}
