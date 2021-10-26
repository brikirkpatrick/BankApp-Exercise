using BankAppLibrary.Accounts;
using System;
using System.Collections.Generic;

namespace BankAppLibrary
{
    public class Bank : IBank
    {
        public string Name { get; }
        public Guid RoutingId { get; }
        public Dictionary<Guid, Account> Accounts { get; private set; }
        public IRoutingSystem RoutingSystem { get; set; }

        public Bank(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException($"{nameof(name)} cannot be null or empty");

            Name = name;
            RoutingId = Guid.NewGuid();
            Accounts = new Dictionary<Guid, Account>();
        }

        /// <summary>
        ///  Searches and returns an account if it exists within the bank.
        /// </summary>
        /// <param name="accId">account id of the account</param>
        /// <returns></returns>
        public Account GetAccountById(Guid accId)
        {
            if (!Accounts.TryGetValue(accId, out Account acc))
            {
                throw new KeyNotFoundException($"bank does not contain an account with id {accId}");
            }

            return acc;
        }

        /// <summary>
        ///  Registers a new checking account and returns a CheckingAcc object which will have relevant account information.
        /// </summary>
        /// <param name="ownerName">name of the account owner</param>
        /// <returns></returns>
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

        /// <summary>
        ///  Registers a new investment account and returns a InvestmentAcc object which will have relevant account information.
        /// </summary>
        /// <param name="ownerName">name of the account owner</param>
        /// <param name="invType">the type of investment account to register</param>
        /// <returns></returns>
        public InvestmentAcc RegisterNewInvestmentAccount(string ownerName, InvestmentAccType invType)
        {
            var newAccId = Guid.NewGuid();
            if (!Accounts.ContainsKey(newAccId))
            {
                InvestmentAcc account;
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
