using BankAppLibrary;
using NUnit.Framework;
using System;

namespace BankAppTests
{
    public class WhenCreatingABank
    {
        private readonly string _bankName = "Default Bank";

        [Test]
        public void InstantiatedBankHasANameAnd0Accounts()
        {
            var bank = new Bank(_bankName);
            Assert.AreEqual(_bankName, bank.Name);
            Assert.IsNotNull(bank.Accounts);
            Assert.AreEqual(0, bank.Accounts.Count);
        }

        [Test]
        public void AndNameIsNullThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new Bank(null));
        }

        [Test]
        [TestCase("CheckingAcc", AccountType.Checking)]
        [TestCase("IndInvestAcc", AccountType.CorportateInvestment)]
        [TestCase("CorpInvestAcc", AccountType.IndividualInvestment)]
        public void AddsANewAccountProperly(string ownerName, AccountType accType)
        {
            var bank = new Bank(_bankName);
            var newAcc = bank.AddNewAccount(ownerName, accType);
            Assert.IsNotNull(newAcc);
            Assert.IsNotNull(newAcc.Id);
            Assert.AreEqual(newAcc.OwnerName, ownerName);
            Assert.AreEqual(newAcc.Type, accType);

            var retrievedAcc = bank.Accounts[newAcc.Id];
            Assert.AreEqual(newAcc, retrievedAcc);
        }

        [Test]
        public void ThrowsExceptionForUnsupportedAccountTypes()
        {
            var bank = new Bank(_bankName);
            Assert.Throws<NotSupportedException>(() => bank.AddNewAccount("UnsupportedAcc", AccountType.Unsupported));
        }
    }
}