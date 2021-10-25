using BankAppLibrary;
using BankAppTests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BankAppTests.AccountTests
{
    // Tests main functionality of the Account class
    public class BaseAccountTests
    {
        private static IBank _mainBank;
        private static IBank _secondBank;
        Dictionary<string, List<Guid>> _accIdDictionary;

        [SetUp]
        public void Setup()
        {
            var routingSystem = new RoutingSystem();
            _mainBank = new Bank("Main Bank");
            _secondBank = new Bank("Secondary Bank");
            routingSystem.RegisterBank(_mainBank);
            routingSystem.RegisterBank(_secondBank);

            _accIdDictionary = new Dictionary<string, List<Guid>>();
            _accIdDictionary.Add(_mainBank.Name, BankingTestsHelper.AddAccountOfEachTypeToBank(_mainBank));
            _accIdDictionary.Add(_secondBank.Name, BankingTestsHelper.AddAccountOfEachTypeToBank(_secondBank));
        }

        // Only for testing withdraw and deposit methods.
        // These objects have no shared bank system state.
        public static IEnumerable<Account> AllAccountTypes
        {
            get
            {
                yield return new CheckingAcc("CheckingOwner", Guid.NewGuid(), new Bank("Test Bank Main"));
                yield return new CorporateInvestmentAcc("CorpInvestOwner", Guid.NewGuid(), new Bank("Test Bank Main"));
                yield return new IndividualInvestmentAcc("IndInvestOwner", Guid.NewGuid(), new Bank("Test Bank Main"));
            }
        }

        [TestCaseSource("AllAccountTypes")]
        public void OnDepositBalanceIsUpdated(Account acc)
        {
            acc.Deposit(500M);
            Assert.AreEqual(500, acc.Balance);

            acc.Deposit(55.55M);
            Assert.AreEqual(555.55, acc.Balance);

            acc.Deposit(12345.10M);
            Assert.AreEqual(12900.65, acc.Balance);
        }

        [TestCaseSource("AllAccountTypes")]
        public void DepositThrowsExceptionWhenAmountIsntPositive(Account acc)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => acc.Deposit(-12345.10M));
        }

        [TestCaseSource("AllAccountTypes")]
        public void OnWithdrawBalanceIsUpdatedAndAmountReturned(Account acc)
        {
            acc.Deposit(600);

            if (acc is IndividualInvestmentAcc)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => acc.Withdraw(501));
                Assert.AreEqual(600, acc.Balance);

                var withdrawnAmount = acc.Withdraw(500);
                Assert.AreEqual(100, acc.Balance);
                Assert.AreEqual(500, withdrawnAmount);

                acc.Deposit(65000.99M);
                withdrawnAmount = acc.Withdraw(500);
                Assert.AreEqual(64600.99, acc.Balance);
                Assert.AreEqual(500, withdrawnAmount);
            }
            else
            {
                var withdrawnAmount = acc.Withdraw(600);
                Assert.AreEqual(0, acc.Balance);
                Assert.AreEqual(600, withdrawnAmount);

                acc.Deposit(65000.99M);
                withdrawnAmount = acc.Withdraw(500);
                Assert.AreEqual(64500.99, acc.Balance);
                Assert.AreEqual(500, withdrawnAmount);

                withdrawnAmount = acc.Withdraw(64500.99M);
                Assert.AreEqual(0, acc.Balance);
                Assert.AreEqual(64500.99M, withdrawnAmount);
            }
        }

        [TestCaseSource("AllAccountTypes")]
        public void WithdrawThrowsExceptionsWhenAmountIsInvalid(Account acc)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => acc.Withdraw(-0.01M));
            Assert.Throws<ArgumentOutOfRangeException>(() => acc.Withdraw(0.01M));
            Assert.Throws<ArgumentOutOfRangeException>(() => acc.Withdraw(0M));
            if (acc is IndividualInvestmentAcc)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => acc.Withdraw(501));
            }
        }

        // TODO: think of more test cases + how to test all account types
        [Test]
        public void InternalTransferSucceeds()
        {
            // Arrange
            var acc1 = _mainBank.GetAccountById(_accIdDictionary[_mainBank.Name][0]);
            acc1.Deposit(500);
            var acc2 = _mainBank.GetAccountById(_accIdDictionary[_mainBank.Name][1]);

            // Act
            acc1.InternalTransfer(acc2.Id, 500);

            //Assert
            Assert.AreEqual(0, acc1.Balance);
            Assert.AreEqual(500, acc2.Balance);
        }

        [Test]
        public void InternalTransferThrowsExceptionWhenFails()
        {
            // Arrange
            var acc1 = _mainBank.GetAccountById(_accIdDictionary[_mainBank.Name][0]);
            acc1.Deposit(500);

            // recipient bank is external
            var externalAcc = new CheckingAcc("Recipient", Guid.NewGuid(), new Bank("NonRegisteredBank"));
            Assert.Throws<KeyNotFoundException>(() => acc1.InternalTransfer(externalAcc.Id, 500));

            // amount isnt positive
            var acc2 = _mainBank.GetAccountById(_accIdDictionary[_mainBank.Name][0]);
            Assert.Throws<ArgumentOutOfRangeException>(() => acc1.InternalTransfer(acc2.Id, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => acc1.InternalTransfer(acc2.Id, -0.01M));
        }

        [Test]
        public void ExternalTransferSucceeds()
        {
            // Arrange
            var mainAcc1 = _mainBank.GetAccountById(_accIdDictionary[_mainBank.Name][0]);
            mainAcc1.Deposit(500);
            var secondaryAcc2 = _secondBank.GetAccountById(_accIdDictionary[_secondBank.Name][0]);

            // Act
            mainAcc1.ExternalTransfer(secondaryAcc2.RoutingId, secondaryAcc2.Id, 500);

            //Assert
            Assert.AreEqual(0, mainAcc1.Balance);
            Assert.AreEqual(500, secondaryAcc2.Balance);
        }

        [Test]
        public void ExternalTransferSucceedsFromIndInvestmentAccAmountGreaterThan500()
        {
            // Arrange
            IndividualInvestmentAcc mainAcc1 = _mainBank.GetAccountById(_accIdDictionary[_mainBank.Name][1]) as IndividualInvestmentAcc;
            mainAcc1.Deposit(600);
            var acc2 = _secondBank.GetAccountById(_accIdDictionary[_secondBank.Name][0]);

            //Act
            mainAcc1.ExternalTransfer(acc2.RoutingId, acc2.Id, 600);

            //Assert
            Assert.AreEqual(0, mainAcc1.Balance);
            Assert.AreEqual(600, acc2.Balance);
        }

        [Test]
        public void ExternalTransferThrowsExceptionsWhenFails()
        {
            // Arrange
            var acc1 = _mainBank.GetAccountById(_accIdDictionary[_mainBank.Name][0]);
            acc1.Deposit(500);
            var acc2 = _secondBank.GetAccountById(_accIdDictionary[_secondBank.Name][0]);

            // bad routingId
            Assert.Throws<KeyNotFoundException>(() => acc1.ExternalTransfer(Guid.NewGuid(), acc2.Id, 500));

            // bad accountId
            Assert.Throws<KeyNotFoundException>(() => acc1.ExternalTransfer(acc2.RoutingId, Guid.NewGuid(), 500));

            // amount > sender Balance
            Assert.Throws<ArgumentOutOfRangeException>(() => acc1.ExternalTransfer(acc2.RoutingId, acc2.Id, 777));

            // amount is not a positive decimal
            Assert.Throws<ArgumentOutOfRangeException>(() => acc1.ExternalTransfer(acc2.RoutingId, acc2.Id, -0.01M));

            // routingSystem is null
            var badBank = new Bank("NonRegisteredBank");
            var acc3 = badBank.RegisterNewCheckingAccount("CheckingAcc");
            acc3.Deposit(500);
            Assert.Throws<NullReferenceException>(() => acc3.ExternalTransfer(acc1.RoutingId, acc1.Id, 500));
        }
    }
}
