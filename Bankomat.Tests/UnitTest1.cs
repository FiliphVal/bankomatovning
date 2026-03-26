using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using ATM;
namespace AtmTest;
 
public class CardTest()
{

    private Account myAccount = new Account (5000);

    [Fact]
    public void RightPinCodeTest()
    {
        Card card1 = new Card("222-222", myAccount);

        bool MatchesPin = card1.MatchesPin("2222");
        Assert.False(MatchesPin);
    }

    [Fact]
    public void WrongPinCodeTest()
    {
        Card card2 = new Card("2022-1212", myAccount);

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

       Assert.Equal(50005, brokeAccount.GetBalance());
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
        Card newCard = new Card("2222", myAccount, "2033");
        atm.InsertCard(newCard);
        Assert.True(atm.HasCardInserted);
        atm.EnterPin("0000");
    }
} 

/* nya funktioner */

public class AddPinTest()
    {
        Account account = new Account(100000);
        AtmService atm = new AtmService(200000);

        [Fact]
        public void AddPin()
    {
        Card newCard = new Card("232323", account);
        atm.InsertCard(newCard);
        Assert.True(atm.HasCardInserted);
        atm.CreatePin("3000");

        Assert.True(atm.EnterPin("3000"));
    }
}
public class DepositIntoATM()
    {
        Account account = new Account(100000);
        AtmService atm = new AtmService(5000);

        [Fact]
        public void DepositTest()
    {
        Card newCard = new Card("232323", account, "2222");
        atm.InsertCard(newCard);
        Assert.True(atm.HasCardInserted);
        Assert.True(atm.EnterPin("2222"));
        atm.AddATMMoney(2000);

        Assert.Equal(7000, atm.GetAtmBalance());
    }
}
public class TypeOfCard()
    {
        Account account = new Account(100000);
        AtmService atm = new AtmService(5000);

        [Fact]
        public void CardTesting()
    {
        Card newCard = new Card("232323", account, "2222", "Gold");
        atm.InsertCard(newCard);
        Assert.True(atm.HasCardInserted);
        Assert.True(atm.EnterPin("2222"));

        Assert.Equal("Gold", atm.CheckCardType());
    }
}

