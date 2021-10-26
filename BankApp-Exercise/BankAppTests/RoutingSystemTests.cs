using BankAppLibrary;
using NUnit.Framework;

namespace BankAppTests
{
    public class RoutingSystemTests
    {
        [Test]
        public void BankRoutingSystemUpdatedOnRegister()
        {
            var routingSystem1 = new RoutingSystem();
            var bank1 = new Bank("Bank 1");

            Assert.IsNull(bank1.RoutingSystem);
            routingSystem1.RegisterBank(bank1);
            Assert.AreEqual(routingSystem1, bank1.RoutingSystem);

            // There should only be 1 routing system but testing that bank1.RoutingSystem is updated on Register
            var routingSystem2 = new RoutingSystem();
            routingSystem2.RegisterBank(bank1);
            Assert.AreEqual(routingSystem2, bank1.RoutingSystem);
        }

        [Test]
        public void ExternalTransferSucceeds()
        {
            var routingSystem = new RoutingSystem();
            var bank1 = new Bank("Bank 1");
            var bank2 = new Bank("Bank 2");
            routingSystem.RegisterBank(bank1);
            routingSystem.RegisterBank(bank2);

            var acc1 = bank1.RegisterNewCheckingAccount("AccOwner1");
            acc1.Deposit(500);
            var acc2 = bank2.RegisterNewCheckingAccount("AccOwner2");

            // Act
            routingSystem.ExternalTransfer(acc1, acc2.RoutingId, acc2.Id, 500);

            // Assert
            Assert.AreEqual(0, acc1.Balance);
            Assert.AreEqual(500, acc2.Balance);
        }

        // TODO: Test that withdrawal is returned should final deposit fail.
        // I explored mocking behavior but struggled to mock internal method functionality.
        [Ignore("TODO Test if possible")]
        [Test]
        public void ExternalTransferReturnsWithdrawOnFailedDeposit()
        {
            // bank2 needs a mockAcc. when mockAcc.Deposit(), throw exception to trigger our exception catch.
            // mock bank2. set routingId. register. 
            // bank2.GetAcc() -> returns mock Acc
            // mockAcc.Withdraw() throws exception


            // register banks
            var routingSystem = new RoutingSystem();
            var bank1 = new Bank("Bank 1");
            var bank2 = new Bank("Bank 2");
            routingSystem.RegisterBank(bank1);
            routingSystem.RegisterBank(bank2);

            // Setup to trigger tested behavior
            //var mockAcc = Substitute.For<IAccount>();
            //mockAcc.When(x => x.Deposit(500)).Do(x => { throw new ArgumentOutOfRangeException(); });
            //bank2.GetAccountById(Arg.Any<Guid>()).Returns(mockAcc);

            var acc1 = bank1.RegisterNewCheckingAccount("CheckingOwner1");
            acc1.Deposit(500);

            //Assert.Throws<ArgumentOutOfRangeException>(() => routingSystem.ExternalTransfer(acc1, bank2.RoutingId, Guid.NewGuid(), 500));

        }
    }
}
