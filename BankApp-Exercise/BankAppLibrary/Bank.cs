using BankAppLibrary.Accounts;
using System;
using System.Collections.Generic;

namespace BankAppLibrary
{
    public class Bank
    {
        public string Name { get; }
        public Guid RoutingId { get; }
        public Dictionary<Guid, Account> Accounts { get; private set; }
        public RoutingSystem RoutingSystem { get; set; }

        public Bank(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException($"{nameof(name)} cannot be null or empty");

            Name = name;
            RoutingId = Guid.NewGuid();
            Accounts = new Dictionary<Guid, Account>();
        }

        public Account GetAccountById(Guid accId)
        {
            if (!Accounts.TryGetValue(accId, out Account acc))
            {
                throw new KeyNotFoundException($"bank does not contain an account with id {accId}");
            }

            return acc;
        }

        // TODO: make key more unique to handle rare cases.
        /// Registers a new checking account provided ownerName.
        /// Returns a CheckingAcc object which will have relevant account information.
        /// The idea is that the user should keep track of this information (name, Id, type) for later use.
        public CheckingAcc RegisterNewCheckingAccount(string ownerName)
        {
            var newAccId = Guid.NewGuid();
            if (!Accounts.ContainsKey(newAccId))
            {
                CheckingAcc account = new CheckingAcc(ownerName, newAccId, this);
                Accounts.Add(account.Id, account);
                return account;
            }
            else
            {
                return null;
            }
        }

        /// Registers a new Investment account provided ownerName and InvestmentAccType.
        /// Returns a InvestmentAccount object which will have relevant account information.
        /// The idea is that the user should keep track of this information (name, Id, type) for later use.
        public InvestmentAccount RegisterNewInvestmentAccount(string ownerName, InvestmentAccType invType)
        {
            var newAccId = Guid.NewGuid();
            if (!Accounts.ContainsKey(newAccId))
            {
                InvestmentAccount account;
                switch (invType)
                {
                    case InvestmentAccType.Corporate:
                        account = new CorporateInvestmentAcc(ownerName, newAccId, this);
                        break;

                    case InvestmentAccType.Individual:
                        account = new IndividualInvestmentAcc(ownerName, newAccId, this);
                        break;
                    default:
                        throw new NotSupportedException($"investment account type '{invType}' is not supported.");
                }

                if (account == null)
                {
                    // TOOD: revisit this exception or handling this null. May not be necessary.
                    throw new NullReferenceException("account is null");
                }

                Accounts.Add(account.Id, account);
                return account;
            }
            else
            {
                return null;
            }
        }
    }
}
