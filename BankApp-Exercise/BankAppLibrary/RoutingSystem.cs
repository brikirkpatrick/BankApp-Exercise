using BankAppLibrary.Accounts;
using System;
using System.Collections.Generic;

namespace BankAppLibrary
{
    // A mediator for communication between banks.
    public class RoutingSystem : IRoutingSystem
    {
        private Dictionary<Guid, IBank> _banks = new Dictionary<Guid, IBank>();

        /// <summary>
        ///  Registers a bank to the routing system.
        /// </summary>
        /// <param name="bank">the bank to register</param>
        public void RegisterBank(IBank bank)
        {
            if (!_banks.ContainsValue(bank))
            {
                _banks[bank.RoutingId] = bank;
            }
            bank.RoutingSystem = this;
        }

        /// <summary>
        ///  Deregisters a bank from the routing system.
        /// </summary>
        /// <param name="bank">the bank to register</param>
        public void DeregisterBank(IBank bank)
        {
            _banks.Remove(bank.RoutingId);
        }

        /// <summary>
        ///  Transfers funds between external accounts.
        /// </summary>
        /// <param name="sender">the sender account</param>
        /// <param name="recipientRoute">recipient routing id</param>
        /// <param name="recipientId">recipient account id</param>
        /// <param name="amount">the amount transferred</param>
        public void ExternalTransfer(Account sender, Guid recipientRoute, Guid recipientId, decimal amount)
        {
            var recipientBank = GetBankByRoutingNum(recipientRoute);
            var recipient = recipientBank.GetAccountById(recipientId);
            decimal withdrawAmt = sender.Withdraw(amount, true);

            try
            {
                recipient.Deposit(withdrawAmt);
            }
            catch(Exception)
            {
                // the deposit failed so re-deposit the withdrawal
                if (withdrawAmt > 0M) sender.Deposit(amount);
                throw;
            }
        }

        private IBank GetBankByRoutingNum(Guid routingId)
        {
            if (!_banks.TryGetValue(routingId, out IBank bank))
            {
                throw new KeyNotFoundException($"bank with routing '{routingId}' has not been registered with the routing system.");
            }

            return bank;
        }
    }
}
