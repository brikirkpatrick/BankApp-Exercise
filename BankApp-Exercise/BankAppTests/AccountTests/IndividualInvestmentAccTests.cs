using BankAppLibrary;
using BankAppLibrary.Accounts;
using NUnit.Framework;
using System;

namespace BankAppTests.AccountTests
{
    // TODO: look into making test classes with shared functionality share a test class?
    // Each acc type class should probably only test functionality that differs?
    public class IndividualInvestmentAccTests
    {
        private IndividualInvestmentAcc _indAcc;
        private Guid _accountId = Guid.NewGuid();
        private readonly string _ownerName = "IndInvestmentOwner";

        [SetUp]
        public void Setup()
        {
            _indAcc = new IndividualInvestmentAcc(_ownerName, _accountId);
        }

        [Test]
        public void ExceptionsAreThrownForBadParams()
        {
            Assert.Throws<ArgumentNullException>(() => new IndividualInvestmentAcc(_ownerName, Guid.Empty));
            Assert.Throws<ArgumentNullException>(() => new IndividualInvestmentAcc(string.Empty, Guid.NewGuid()));
            Assert.Throws<ArgumentNullException>(() => new IndividualInvestmentAcc(null, Guid.NewGuid()));
            Assert.Throws<ArgumentNullException>(() => new IndividualInvestmentAcc(null, Guid.Empty));
        }

        [Test]
        public void ConstructorIsProperlyInitialized()
        {
            Assert.IsNotNull(_indAcc.Id);
            Assert.AreEqual(_ownerName, _indAcc.OwnerName);
            Assert.AreEqual(0, _indAcc.Balance);
            Assert.IsNotNull(_indAcc.Id);
            Assert.AreEqual(AccountType.Investment, _indAcc.Type);
            Assert.AreEqual(InvestmentAccType.Individual, _indAcc.InvestmentType);
        }

        [Test]
        public void OnWithdrawBalanceIsUpdatedAndAmountReturned()
        {
            _indAcc.Deposit(500);
            var withdrawnAmount = _indAcc.Withdraw(500);
            Assert.AreEqual(0, _indAcc.Balance);
            Assert.AreEqual(500, withdrawnAmount);

            _indAcc.Deposit(65000.99M);
            withdrawnAmount = _indAcc.Withdraw(500);
            Assert.AreEqual(64500.99, _indAcc.Balance);
            Assert.AreEqual(500, withdrawnAmount);

            withdrawnAmount = _indAcc.Withdraw(50);
            Assert.AreEqual(64450.99m, _indAcc.Balance);
            Assert.AreEqual(50m, withdrawnAmount);
        }

        [Test]
        public void WithdrawThrowsExceptionsWhenAmountIsInvalid()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _indAcc.Withdraw(600M));
            Assert.Throws<ArgumentException>(() => _indAcc.Withdraw(-0.01M));
            Assert.Throws<InvalidOperationException>(() => _indAcc.Withdraw(0.01M));
        }
    }
}
