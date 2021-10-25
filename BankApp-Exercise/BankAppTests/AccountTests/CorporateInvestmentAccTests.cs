using BankAppLibrary;
using BankAppLibrary.Accounts;
using NUnit.Framework;
using System;

namespace BankAppTests.AccountTests
{
    public class CorporateInvestmentAccTests
    {
        private CorporateInvestmentAcc _corpAcc;
        private IBank _testBank;
        private Guid _accountId = Guid.NewGuid();
        private readonly string _ownerName = "CorpInvestmentOwner";

        [SetUp]
        public void Setup()
        {
            _testBank = new Bank("Test Bank");
            _corpAcc = new CorporateInvestmentAcc(_ownerName, _accountId, _testBank);
        }

        [Test]
        public void BadConstructorParamsThrowExceptions()
        {
            Assert.Throws<ArgumentNullException>(() => new CorporateInvestmentAcc(_ownerName, Guid.Empty, new Bank("Test Bank")));
            Assert.Throws<ArgumentNullException>(() => new CorporateInvestmentAcc(string.Empty, _accountId, _testBank));
            Assert.Throws<ArgumentNullException>(() => new CorporateInvestmentAcc(null, _accountId, _testBank));
            Assert.Throws<ArgumentNullException>(() => new CorporateInvestmentAcc(_ownerName, _accountId, null));
            Assert.Throws<ArgumentNullException>(() => new CorporateInvestmentAcc(null, Guid.Empty, null));
        }

        [Test]
        public void AccountIsProperlyInitialized()
        {
            Assert.IsNotNull(_corpAcc);
            Assert.IsNotNull(_corpAcc.Id);
            Assert.IsNotNull(_corpAcc.RoutingId);
            Assert.AreEqual(_ownerName, _corpAcc.OwnerName);
            Assert.AreEqual(0, _corpAcc.Balance);
            Assert.AreEqual(AccountType.Investment, _corpAcc.Type);
            Assert.AreEqual(InvestmentAccType.Corporate, _corpAcc.InvestmentType);
        }
    }
}
