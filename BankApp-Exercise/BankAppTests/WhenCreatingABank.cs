using BankAppLibrary;
using BankAppLibrary.Accounts;
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
        public void InstantiatedBankHasExpectedPropertiesSet()
        {
            Assert.AreEqual(_bankName, _bank.Name);
            Assert.IsNotNull(_bank.RoutingId);
            Assert.IsNotNull(_bank.Accounts);
            Assert.IsNull(_bank.RoutingSystem);
            Assert.AreEqual(0, _bank.Accounts.Count);
        }

        [Test]
        public void AndNameIsNullOrEmptyThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new Bank(null));
            Assert.Throws<ArgumentNullException>(() => new Bank(string.Empty));
        }

        [Test]
        public void AddsANewCheckingAccountProperly()
        {
            var ownerName = "CheckingAcc";
            var newAcc = _bank.RegisterNewCheckingAccount(ownerName);
            Assert.IsNotNull(newAcc);
            Assert.IsNotNull(newAcc.Id);
            Assert.AreEqual(ownerName, newAcc.OwnerName);
            Assert.AreEqual(AccountType.Checking, newAcc.Type);

            var retrievedAcc = _bank.Accounts[newAcc.Id];
            Assert.AreEqual(newAcc, retrievedAcc);
        }

        [Test]
        [TestCase("IndInvestAcc", InvestmentAccType.Individual)]
        [TestCase("CorpInvestAcc", InvestmentAccType.Corporate)]
        public void AddsANewInvestmentAccountProperly(string ownerName, InvestmentAccType invType)
        {
            var newAcc = _bank.RegisterNewInvestmentAccount(ownerName, invType);
            Assert.IsNotNull(newAcc);
            Assert.IsNotNull(newAcc.Id);
            Assert.AreEqual(ownerName, newAcc.OwnerName);
            Assert.AreEqual(invType, newAcc.InvestmentType);
            Assert.AreEqual(AccountType.Investment, newAcc.Type);

            var retrievedAcc = _bank.Accounts[newAcc.Id];
            Assert.AreEqual(newAcc, retrievedAcc);
        }

        [Test]
        public void AddNewInvestAccThrowsExceptionForUnsupportedType()
        {
            Assert.Throws<NotSupportedException>(() => _bank.RegisterNewInvestmentAccount("UnsupportedAcc", InvestmentAccType.Unsupported));
        }
    }
}