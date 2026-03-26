namespace ATM;

public class AtmService
{
    private Card? _currentCard;
    private bool _isAuthenticated;

    public bool HasCardInserted => _currentCard != null;
    public bool IsAuthenticated => _isAuthenticated;
    
    public int AtmBalance { get; private set; }

    public AtmService(int initialBalance)
    {
        AtmBalance = initialBalance;
    }
    public int GetAtmBalance()
    {
        EnsureAuthenticated();
        return AtmBalance;
    }


    public void InsertCard(Card card)
    {
        _currentCard = card;
        _isAuthenticated = false;
    }

    public void EjectCard()
    {
        _currentCard = null;
        _isAuthenticated = false;
    }


    public bool EnterPin(string pinCode)
    {
        if (_currentCard == null)
        {
            return false;
        }
        if (_currentCard.MatchesPin(pinCode))
        {
        _isAuthenticated = _currentCard.MatchesPin(pinCode);
        _failedAttemps = 0;
        return _isAuthenticated;
        }
        else
        {
            _failedAttemps++;
            if(_failedAttemps >= 3)
            {
                EjectCard();

            }
            return false;
        }
    }

    public int GetBalance()
    {
        EnsureAuthenticated();
        return _currentCard!.Account.GetBalance();
    }


    public bool Withdraw(int amount)
    {
        EnsureAuthenticated();
        if (amount > AtmBalance)
        {
            return false;
        }
        bool success = _currentCard!.Account.Withdraw(amount);
        if (success)
        {
            AtmBalance -= amount;
        }

        return success;

    }
    
    public bool Deposit(int amount)
    {
        EnsureAuthenticated();
        return _currentCard!.Account.Deposit(amount);
    }
    
    public void EnsureAuthenticated()
    {
        if (_currentCard == null || !_isAuthenticated)
        {
            throw new InvalidOperationException("Ingen autentiserad session.");
        }
    }

    /* ================================== NYA METODER ================================== */

    private int _failedAttemps = 0;
    public int GetRemainingAttempts()
    {
        return 3 - _failedAttemps;
    }
    public bool AddATMMoney(int AddedMoney)
    {
        AtmBalance += AddedMoney;
        return true;
    }


    public void WriteBalance()
    {
        EnsureAuthenticated();
        Console.WriteLine("Du har " + _currentCard.Account.GetBalance() + " kr på ditt konto!");
    }

    public bool CreatePin(string newPin)
    {
        if(_currentCard == null)
        {
            return false;
        }
        if(_currentCard.PinCode != null)
        {
            return false;
        }

        _currentCard.SetPin(newPin);
        _isAuthenticated = true;
        return true;
    }

    public bool ChangePin(string newPin)
    {
        if(_currentCard == null || _currentCard.PinCode == null)
        {
            return false;
        }
        _currentCard.SetPin(newPin);
        _isAuthenticated = true;
        return true;

    }
    public bool CurrentCardNeedsPinCode()
    {
        if (_currentCard.PinCode == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public string CheckCardType()
        {
            EnsureAuthenticated();
            return _currentCard!.WhatCardType();
        }
}
