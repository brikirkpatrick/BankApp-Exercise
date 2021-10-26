using BankAppLibrary;
using BankAppLibrary.Accounts;
using NUnit.Framework;
using System;

namespace BankAppTests.AccountTests
{
    public class CheckingAccountTests
    {
        private CheckingAcc _checkingAcc;
        private IBank _testBank;
        private Guid _accountId = Guid.NewGuid();
        private readonly string _ownerName = "CheckingOwner";

        [SetUp]
        public void Setup()
        {
            _testBank = new Bank("Test Bank");
            _checkingAcc = new CheckingAcc(_ownerName, _accountId, _testBank);
        }

        [Test]
        public void BadConstructorParamsThrowExceptions()
        {
            Assert.Throws<ArgumentNullException>(() => new CheckingAcc(_ownerName, Guid.Empty, new Bank("Test Bank")));
            Assert.Throws<ArgumentNullException>(() => new CheckingAcc(string.Empty, _accountId, _testBank));
            Assert.Throws<ArgumentNullException>(() => new CheckingAcc(null, _accountId, _testBank));
            Assert.Throws<ArgumentNullException>(() => new CheckingAcc(_ownerName, _accountId, null));
            Assert.Throws<ArgumentNullException>(() => new CheckingAcc(null, Guid.Empty, null));
        }

        [Test]
        public void AccountIsProperlyInitialized()
        {
            Assert.IsNotNull(_checkingAcc);
            Assert.IsNotNull(_checkingAcc.Id);
            Assert.IsNotNull(_checkingAcc.RoutingId);
            Assert.AreEqual(_ownerName, _checkingAcc.OwnerName);
            Assert.AreEqual(0, _checkingAcc.Balance);
            Assert.AreEqual(AccountType.Checking, _checkingAcc.Type);
        }
    }
}
