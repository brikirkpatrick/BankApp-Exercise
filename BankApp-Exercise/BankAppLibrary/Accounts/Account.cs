using System;

namespace BankAppLibrary
{
    public enum AccountType
    {
        Checking,
        Investment
    }

    // TODO: Make note of passed considerations in ReadMe
    // - Overdraft
    // - OwnerName + BankName sanitization

    // TODO: proper code comments for intellisense (see Deposit)
    // TODO: Big, how to handle decimal numbers where more than 2 decimal points? remember to adjust tests for this.
    // TODO: consider abstracts folder or project for interfaces and enums?
    // TODO: is there ANY benefit to an IAccount interface?
    // TODO: Should we be able to RegisterAcc if RoutingSystem is null? I think yes...
    public abstract class Account
    {
        private IBank _bank;
        public string OwnerName { get; }
        public Guid Id { get; }
        public Guid RoutingId { get; }
        public AccountType Type { get; }
        public decimal Balance { get; protected set; }

        // Note: Taking in Bank is kinda weird to me. 
        // Needed for Transfers (Internal Accs + RoutingSystem for external accounts)
        // Maybe realistically I'd take in like an accountRepository or something.
        public Account(string ownerName, Guid id, AccountType type, IBank bank)
        {
            if (string.IsNullOrWhiteSpace(ownerName)) throw new ArgumentNullException(nameof(ownerName), "cannot be null or empty");
            if (id == null || id == Guid.Empty) throw new ArgumentNullException(nameof(id), "cannot be null or empty");
            if (bank == null) throw new ArgumentNullException(nameof(bank), "cannot be null.");

            _bank = bank;
            RoutingId = _bank.RoutingId; // TODO: what if RoutingId is null?
            OwnerName = ownerName;
            Id = id;
            Type = type;
            Balance = 0;
        }

        /// <summary>
        ///  Deposits a positive amount to an account.
        /// </summary>
        /// <param name="amount">a positive decimal.</param>
        public virtual void Deposit(decimal amount) 
        {
            if (amount <= 0M) throw new ArgumentOutOfRangeException($"{nameof(amount)} must be a positive decimal.");

            Balance = Balance + amount;
        }

        /// Withdraws a specified positive amount from an account and returns the amount withdrawn.
        /// isTransferring is an optional parameter for handling transfer-withdraw cases.
        public virtual decimal Withdraw(decimal amount, bool isTransferring = false)
        {
            if (amount <= 0M) throw new ArgumentOutOfRangeException(nameof(amount), "amount must be a positive decimal.");
            if (amount > Balance) throw new ArgumentOutOfRangeException(nameof(amount), "amount to withdraw is greater than current account balance.");

            Balance = Balance - amount;
            return amount;
        }

        /// Transfer an amount internally provided recipient account Id.
        public virtual void InternalTransfer(Guid recipientId, decimal amount) 
        {
            var receiver = _bank.GetAccountById(recipientId);
            decimal withdrawAmt = this.Withdraw(amount, true);
            receiver.Deposit(withdrawAmt);
        }

        /// Transfer an amount externally provided recipient routingId and accountId.
        public virtual void ExternalTransfer(Guid recipientRoute, Guid recipientId, decimal amount)
        {
            if (_bank.RoutingSystem == null)
            {
                throw new NullReferenceException("the bank associated with this account is not registered to a routingSystem.");
            }

            _bank.RoutingSystem.ExternalTransfer(this, recipientRoute, recipientId, amount);
        }
    }
}
