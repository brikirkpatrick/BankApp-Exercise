using BankAppLibrary;
using BankAppLibrary.Accounts;
using NUnit.Framework;
using System;

namespace BankAppTests.AccountTests
{
    public class IndividualInvestmentAccTests
    {
        private IndividualInvestmentAcc _indAcc;
        private IBank _testBank;
        private Guid _accountId = Guid.NewGuid();
        private readonly string _ownerName = "IndInvestmentOwner";

        [SetUp]
        public void Setup()
        {
            _testBank = new Bank("Test Bank");
            _indAcc = new IndividualInvestmentAcc(_ownerName, _accountId, _testBank);
        }

        [Test]
        public void BadConstructorParamsThrowExceptions()
        {
            Assert.Throws<ArgumentNullException>(() => new IndividualInvestmentAcc(_ownerName, Guid.Empty, _testBank));
            Assert.Throws<ArgumentNullException>(() => new IndividualInvestmentAcc(string.Empty, _accountId, _testBank));
            Assert.Throws<ArgumentNullException>(() => new IndividualInvestmentAcc(null, _accountId, _testBank));
            Assert.Throws<ArgumentNullException>(() => new IndividualInvestmentAcc(_ownerName, _accountId, null));
            Assert.Throws<ArgumentNullException>(() => new IndividualInvestmentAcc(null, Guid.Empty, null));
        }

        [Test]
        public void ConstructorIsProperlyInitialized()
        {
            Assert.IsNotNull(_indAcc);
            Assert.IsNotNull(_indAcc.Id);
            Assert.IsNotNull(_indAcc.RoutingId);
            Assert.AreEqual(_ownerName, _indAcc.OwnerName);
            Assert.AreEqual(0, _indAcc.Balance);
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
            Assert.Throws<ArgumentOutOfRangeException>(() => _indAcc.Withdraw(-0.01M));
            Assert.Throws<ArgumentOutOfRangeException>(() => _indAcc.Withdraw(0));
        }
    }
}
