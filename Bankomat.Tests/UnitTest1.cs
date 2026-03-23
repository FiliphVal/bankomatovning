using ATM;
namespace AtmTest;

public class AccountTest
{
    private Account account = new Account(5000);

    [Fact]
    public void DepositTest()
    {
        account.Deposit(5000);
        Assert.Equal(10000, account.GetBalance());
    }

    [Fact]
    public void WithdrawTest()
    {
        account.Withdraw(5000);
        Assert.Equal(0, account.GetBalance());
    }

    /* public void WithdrawFailTest()
    {
        account.Withdraw(20000);
        Assert.Equal(5000, account.GetBalance());
    } */
}

public class CardTest
{
    private Account myAccount = new Account(50000);

    [Fact]

    public void RightPinCode()
    {
        var card = new Card("12345", "9322", myAccount);

        bool result = card.MatchesPin("9322");

        Assert.True(result);
    }

    [Fact]
    public void WrongPinCode()
    {
        var card = new Card("1234", "2222", myAccount);

        bool result = card.MatchesPin("9999");

        Assert.False(result);
    }
}

public class AtmServiceTest
{

    [Fact]
    public void RightPinCodeEnoughMoney()
    {
        var myAccount = new Account(1000);
        var richCard = new Card("9322-2", "9333", myAccount);
        var atm = new AtmService(2000);

        atm.InsertCard(richCard);
        bool Pin = atm.EnterPin("9333");
        bool Withdraw = atm.Withdraw(500);

        Assert.True(Pin);
        Assert.True(Withdraw);
        Assert.Equal(500, myAccount.GetBalance());
        Assert.Equal(1500, atm.AtmBalance);
    }

    [Fact]

    public void TestUppgift()
    {
        var account = new Account(9000);
        var card = new Card("9333", "0000", account);
        var atm = new AtmService(11000);
        atm.InsertCard(card);
        bool wrongPin = atm.EnterPin("1234");
        bool rightPin = atm.EnterPin("0000");
        bool Withdraw = atm.Withdraw(5000);
        atm.EjectCard();
        atm.InsertCard(card);
        bool rightPin2 = atm.EnterPin("0000");
        bool WithdrawBroke = atm.Withdraw(7000);
        bool WithdrawRight = atm.Withdraw(6000);
        atm.EjectCard();


        Assert.False(wrongPin);
        Assert.True(rightPin);
        Assert.Equal(4000, account.GetBalance());
        Assert.Equal(6000, atm.AtmBalance);
        Assert.True(rightPin2);
        Assert.Equal(4000, account.GetBalance());
        Assert.Equal(6000, atm.AtmBalance);
        Assert.Equal(4000, account.GetBalance());
        Assert.Equal(6000, atm.AtmBalance);

    }
}
/*
Bankomaten har 11000 kr. På kontot finns 9000 kr.

Sätt in ett kort i bankomaten. (Bankomaten ska veta att ett kort är inne)
Mata in en felaktig pinkod (1234) i bankomaten. (Pinkoden ska vara sparad på kortet, men det är bankomaten som ska veta om rätt eller fel kod matats in)
Mata in korrekt pinkod (0123).
Ange 5000 kr att ta ut via bankomaten. Balansen ska tas från kontot som är kopplat till kortet.
Mata ut kortet ur bankomaten.
Mata in kortet igen, slå in pinkoden igen.
Ange 7000 att ta ut. (Nu ska det inte finnas pengar så det räcker på bankomaten)
Ange 6000 att ta ut. (Nu räckte bankomatens pengar precis, men inte pengarna på kontot)
Mata ut kortet ur bankomaten.

*/