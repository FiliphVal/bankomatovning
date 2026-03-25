namespace ATM;

public class Account
{
    public int Balance { get; private set; }

    public Account(int initialBalance)
    {
        Balance = initialBalance;
    }

    public int GetBalance()
    {
        return Balance;
    }

    public bool Withdraw(int amount)
    {
        if (amount > Balance)
        {
            return false;
        }

        Balance -= amount;
        return true;
    }
    
    public bool Deposit(int amount)
    {
        if(amount <= 0)
        {
            return false;
        }
        
        Balance += amount;
        return true;
    }
    
}