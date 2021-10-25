using System;
using System.Collections.Generic;

namespace BankAppLibrary
{
    // Mediator Class https://www.dofactory.com/net/mediator-design-pattern
    public class RoutingSystem : IRoutingSystem
    {
        private Dictionary<Guid, IBank> _banks = new Dictionary<Guid, IBank>();

        public void RegisterBank(IBank bank)
        {
            if (!_banks.ContainsValue(bank))
            {
                _banks[bank.RoutingId] = bank;
            }
            bank.RoutingSystem = this;
        }

        public void ExternalTransfer(Account sender, Guid recipientRoute, Guid recipientId, decimal amount)
        {
            decimal withdrawAmt = 0M;
            var recipientBank = GetBankByRoutingNum(recipientRoute);
            var recipient = recipientBank.GetAccountById(recipientId);

            try
            {
                withdrawAmt = sender.Withdraw(amount, true);
                recipient.Deposit(withdrawAmt);
            }
            catch(Exception ex)
            {
                // TODO: see if possible tot est for this balance not changing.
                if (ex is ArgumentOutOfRangeException || ex is InvalidOperationException)
                {
                    // the deposit failed so re-deposit the withdrawal
                    if (withdrawAmt != 0M) sender.Deposit(amount);
                }
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
