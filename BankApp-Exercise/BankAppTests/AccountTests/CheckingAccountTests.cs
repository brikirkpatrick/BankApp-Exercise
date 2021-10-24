using BankAppLibrary;
using NUnit.Framework;
using System;

namespace BankAppTests
{
    public class WhenCreatingACheckingAccount
    {
        private CheckingAcc _checkingAcc;
        private Guid _accountId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            _checkingAcc = new CheckingAcc("CheckingOwner", _accountId);
        }

        [Test]
        public void ExceptionsAreThrownForBadParams()
        {
            Assert.Throws<ArgumentNullException>(() => new CheckingAcc("CheckingOwner", Guid.Empty));
            Assert.Throws<ArgumentNullException>(() => new CheckingAcc(string.Empty, Guid.NewGuid()));
            Assert.Throws<ArgumentNullException>(() => new CheckingAcc(null, Guid.NewGuid()));
            Assert.Throws<ArgumentNullException>(() => new CheckingAcc(null, Guid.Empty));
        }

        [Test]
        public void ConstructorIsProperlyInitialized()
        {
            Assert.IsNotNull(_checkingAcc.Id);
            Assert.AreEqual("CheckingOwner", _checkingAcc.OwnerName);
            Assert.AreEqual(0, _checkingAcc.Balance);
            Assert.AreEqual(AccountType.Checking, _checkingAcc.Type);
            Assert.IsNotNull(_checkingAcc.Id);
        }

        [Test]
        public void OnDepositBalanceIsUpdated()
        {
            _checkingAcc.Deposit(500M);
            Assert.AreEqual(500, _checkingAcc.Balance);

            _checkingAcc.Deposit(55.55M);
            Assert.AreEqual(555.55, _checkingAcc.Balance);

            _checkingAcc.Deposit(12345.10M);
            Assert.AreEqual(12900.65, _checkingAcc.Balance);

            Assert.Throws<ArgumentException>(() => _checkingAcc.Deposit(-12345.10M));
        }

        [Test]
        public void DepositThrowsExceptionWhenAmountIsNegative()
        {
            Assert.Throws<ArgumentException>(() => _checkingAcc.Deposit(-12345.10M));
        }

        [Test]
        public void OnWithdrawBalanceIsUpdatedAndAmountReturned()
        {
            _checkingAcc.Deposit(500);
            var withdrawnAmount= _checkingAcc.Withdraw(500);

            Assert.AreEqual(0, _checkingAcc.Balance);
            Assert.AreEqual(500, withdrawnAmount);

            _checkingAcc.Deposit(65000.99M);
            withdrawnAmount = _checkingAcc.Withdraw(500);
            Assert.AreEqual(64500.99, _checkingAcc.Balance);
            Assert.AreEqual(500, withdrawnAmount);

            withdrawnAmount = _checkingAcc.Withdraw(64500.99M);
            Assert.AreEqual(0, _checkingAcc.Balance);
            Assert.AreEqual(64500.99M, withdrawnAmount);
        }

        [Test]
        public void WithdrawThrowsExceptionsWhenAmountIsInvalid()
        {
            Assert.Throws<ArgumentException>(() => _checkingAcc.Withdraw(-0.01M));
            Assert.Throws<InvalidOperationException>(() => _checkingAcc.Withdraw(0.01M));
        }
    }
}
