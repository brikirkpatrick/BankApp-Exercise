using System;
using System.Collections.Generic;

namespace BankAppLibrary
{
    public class Bank : IBank
    {
        public string Name { get; }
        public Dictionary<Guid, Account> Accounts { get; private set; }


        public Bank(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException($"{nameof(name)} cannot be null or empty");


            Name = name;
            Accounts = new Dictionary<Guid, Account>();
        }

        // TODO: make key more unique to handle rare cases.
        /*
         * Creates a new account using the owner name and requested account type.
         * Returns an Account object which will have relevant account information.
         * The idea is that the user should keep track of this information (name, Id, type) for later use.
         */
        public Account AddNewAccount(string ownerName, AccountType type)
        {
            var newAccId = Guid.NewGuid();
            if (!Accounts.ContainsKey(newAccId))
            {
                Account account;
                switch (type)
                {
                    case AccountType.Checking:
                        account = new CheckingAcc(ownerName, newAccId);
                        break;

                    case AccountType.CorportateInvestment:
                        account = new CorporateInvestmentAcc(ownerName, newAccId);
                        break;

                    case AccountType.IndividualInvestment:
                        account = new IndividualInvestmentAcc(ownerName, newAccId);
                        break;
                    default:
                        throw new NotSupportedException($"Account Type '{type}' is not supported.");
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

        public bool DepositToAccount(Guid accId, float amount)
        {
            throw new NotImplementedException();
        }

        public bool WithdrawFromAccount(Guid accId, float amount)
        {
            throw new NotImplementedException();
        }

        public bool TransferFromAccToAcc(Guid senderId, Guid receiverId, Bank bank, float amount)
        {
            throw new NotImplementedException();
        }

        //Potential Methods
        // DepositToAccount
        // get account, call account.Withdraw

        // WithdrawFromAccount
        // get account, call account.Deposit

        // TransferFromAccToAcc (tricky because require access to another bank)
        // get fromAcc, toAcc, amount, Bank
        // checking that withdraw is handled properly for certain accounts
    }
}
