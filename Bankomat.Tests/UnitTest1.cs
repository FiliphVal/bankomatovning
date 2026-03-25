using ATM;
namespace AtmTest;
 
public class CardTest()
{

    private Account myAccount = new Account (5000);

    [Fact]
    public void RightPinCodeTest()
    {
        Card card1 = new Card("222-222", "2222", myAccount);

        bool MatchesPin = card1.MatchesPin("2222");
        Assert.True(MatchesPin);
    }

    [Fact]
    public void WrongPinCodeTest()
    {
        Card card2 = new Card("2022-1212", "9343", myAccount);

        bool noMatch = card2.MatchesPin("0303");

        Assert.False(noMatch);
    }
}

public class AccountTest()
{

    private Account brokeAccount = new Account(5);
    [Fact]
    public void BigDeposit()
    {
       brokeAccount.Deposit(50000);

       Assert.Equal(50000, brokeAccount.GetBalance());
    }


  private Account RichAccount = new Account(50000);

    [Fact]
    public void ToBigWithdraw()
    {
        RichAccount.Withdraw(50005);

        Assert.Equal(50000, RichAccount.GetBalance());
    }

    private Account myAccount2 = new Account(20000);
    [Fact]
    public void Withdraw()
    {
        myAccount2.Withdraw(5500);

        Assert.Equal(14500, myAccount2.GetBalance());
    }
}

public class ATMTest()
{
    private Account myAccount = new Account(5000);
    AtmService atm = new AtmService(20000);
    [Fact]
    public void ATMTesting()
    {
        Card newCard = new Card("2222","0000", myAccount);
        atm.InsertCard(newCard);
        Assert.True(atm.HasCardInserted);
        atm.EnterPin("0000");

    }
} 