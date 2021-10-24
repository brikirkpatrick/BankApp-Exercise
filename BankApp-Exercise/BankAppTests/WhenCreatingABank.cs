using BankAppLibrary;
using NUnit.Framework;
using System;

namespace BankAppTests
{
    public class WhenCreatingABank
    {
        private readonly string _bankName = "Default Bank";
        private Bank _bank;

        [SetUp]
        public void Setup()
        {
            _bank = new Bank(_bankName);
        }

        [Test]
        public void InstantiatedBankHasANameAnd0Accounts()
        {
            Assert.AreEqual(_bankName, _bank.Name);
            Assert.IsNotNull(_bank.Accounts);
            Assert.AreEqual(0, _bank.Accounts.Count);
        }

        [Test]
        public void AndNameIsNullOrEmptyThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new Bank(null));
            Assert.Throws<ArgumentNullException>(() => new Bank(string.Empty));
        }

        [Test]
        [TestCase("CheckingAcc", AccountType.Checking)]
        [TestCase("IndInvestAcc", AccountType.CorportateInvestment)]
        [TestCase("CorpInvestAcc", AccountType.IndividualInvestment)]
        public void AddsANewAccountProperly(string ownerName, AccountType accType)
        {
            var newAcc = _bank.AddNewAccount(ownerName, accType);
            Assert.IsNotNull(newAcc);
            Assert.IsNotNull(newAcc.Id);
            Assert.AreEqual(newAcc.OwnerName, ownerName);
            Assert.AreEqual(newAcc.Type, accType);

            var retrievedAcc = _bank.Accounts[newAcc.Id];
            Assert.AreEqual(newAcc, retrievedAcc);
        }

        [Test]
        public void AddNewAccThrowsExceptionForUnsupportedAccountTypes()
        {
            Assert.Throws<NotSupportedException>(() => _bank.AddNewAccount("UnsupportedAcc", AccountType.Unsupported));
        }
    }
}