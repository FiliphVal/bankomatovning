namespace ATM;

public class Card
{
    public string CardNumber { get; }
    public string? PinCode;
    public string ? CardType;
    public Account Account { get; }

    public Card(string cardNumber, Account account, string? pinCode = null, string? cardType = null)
    {
        CardNumber = cardNumber;
        PinCode = pinCode;
        CardType = cardType;
        Account = account;
    }

    public void SetPin(string pin)
    {
        PinCode = pin;
    }

    public bool MatchesPin(string pinCode)
    {
        return PinCode == pinCode;
    }

    public string WhatCardType()
    {
        return CardType;
    }
}