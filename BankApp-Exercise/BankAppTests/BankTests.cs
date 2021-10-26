using BankAppLibrary;
using BankAppLibrary.Accounts;
using NUnit.Framework;
using System;

namespace BankAppTests
{
    public class BankTests
    {
        private readonly string _bankName = "Default Bank";
        private IBank _bank;

        [SetUp]
        public void Setup()
        {
            _bank = new Bank(_bankName);
        }

        [Test]
        public void BankIsProperlyInitialized()
        {
            Assert.IsNotNull(_bank);
            Assert.AreEqual(_bankName, _bank.Name);
            Assert.IsNotNull(_bank.RoutingId);
            Assert.IsNotNull(_bank.Accounts);
            Assert.AreEqual(0, _bank.Accounts.Count);
            Assert.IsNull(_bank.RoutingSystem);
        }

        [Test]
        public void BadConstructorParamsThrowExceptions()
        {
            Assert.Throws<ArgumentNullException>(() => new Bank(null));
            Assert.Throws<ArgumentNullException>(() => new Bank(string.Empty));
        }

        [Test]
        public void RegistersANewCheckingAccountProperly()
        {
            var ownerName = "CheckingOwner";
            var newAcc = _bank.RegisterNewCheckingAccount(ownerName);
            Assert.IsNotNull(newAcc);
            Assert.IsNotNull(newAcc.Id);
            Assert.AreEqual(ownerName, newAcc.OwnerName);
            Assert.AreEqual(AccountType.Checking, newAcc.Type);

            var retrievedAcc = _bank.Accounts[newAcc.Id];
            Assert.AreEqual(newAcc, retrievedAcc);
        }

        [Test]
        [TestCase("IndInvestOwner", InvestmentAccType.Individual)]
        [TestCase("CorpInvestOwner", InvestmentAccType.Corporate)]
        public void RegistersNewInvestmentAccountsProperly(string ownerName, InvestmentAccType invType)
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
        public void RegisterNewInvestAccThrowsExceptionForUnsupportedType()
        {
            Assert.Throws<NotSupportedException>(() => _bank.RegisterNewInvestmentAccount("UnsupportedOwner", InvestmentAccType.Unsupported));
        }
    }
}