namespace ATM;

using System.Globalization;
using AtmTest;

public class ConsoleRunner
{
    private AtmService _atmService;
    List<Card> _cards = new List<Card>();
    List<string> WithdrawReceipts = new List<string>();
    List<string> DepositReceipts = new List<string>();

    public ConsoleRunner(AtmService atmService)
    {
        _atmService = atmService;
    Account account1 = new Account(20000);
    Account account2 = new Account(350000);
        
     _cards.Add(new Card("1111", account1, "1111", "Gold"));
     _cards.Add(new Card("2222", account2, null, "Platinum"));
    }

    

    public void Run()
    {
        bool running = true;

        while (running)
        {
            if (!_atmService.HasCardInserted)
            {
                running = ShowWelcomeMenu();
            }
            else
            {
                ShowMainMenu();
            }
        }
        
        Console.WriteLine("Tack och hej!");
        
    }

    private bool ShowWelcomeMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== BANKOMAT ===");
        Console.WriteLine("1. Mata in kort");
        Console.WriteLine("0. Avsluta");
        Console.Write("Val: ");

        string? input = Console.ReadLine();

        switch (input)
        {
            case "1":
            Console.WriteLine("Ange kortnummer");
            string? inputKortnummer = Console.ReadLine();

            Card? foundCard = null;

                foreach(Card c in _cards)
                {
                    if(c.CardNumber == inputKortnummer)
                    {
                        foundCard = c;
                    }
                }

                if(foundCard != null)
                {
                    Console.WriteLine("Kort inmatat.");
                    _atmService.InsertCard(foundCard);
                    ShowPinMenu(foundCard);
                }
                else
                {
                    Console.WriteLine("Fel kortnummer inmatat.");
                }

            return true;

            case "0":
                return false;

            default:
                Console.WriteLine("Ogiltigt val.");
                return true;
        }
    }

    private void ShowPinMenu(Card current)
    {
        if(_atmService.CurrentCardNeedsPinCode())
        {
            System.Console.WriteLine("========= SKAPA PINKOD ============");
            Console.WriteLine("Det verkar som att du saknar en pinkod.");
            Console.WriteLine("Du kommer nu bli ombedd att skapa en pinkod.");
            System.Console.WriteLine("Vänligen ange en 4 siffrig pinkod: \n ");
            string? newPin = Console.ReadLine();

            if(newPin != null)
            {
                _atmService.CreatePin(newPin);
                System.Console.WriteLine("Din nya pinkod: " + newPin + " har nu sparats till ditt kort");
                ShowMainMenu();
            }
            else
            {
                Console.WriteLine("Kunde ej skapa pinkod.");
                _atmService.EjectCard();
            }

        }
        else
        {
         bool EnteringPin =true;
         while (EnteringPin)
            {
            Console.WriteLine();
            Console.Write("Ange PIN: ");
            string? pin = Console.ReadLine();

            bool ok = _atmService.EnterPin(pin ?? "");

            if (ok)
            {
                Console.WriteLine("PIN korrekt.");
                break;
            }
            else
            {
                Console.WriteLine("Fel PIN.");
                int kvar = _atmService.GetRemainingAttempts();
                System.Console.WriteLine($"Du har {kvar} försök kvar på dig innan kortet spärras.");
                if(kvar <= 0 && current != null)
                    {
                        System.Console.WriteLine("======== KORTET SPÄRRAT =========");
                        _atmService.EjectCard();
                        _cards.Remove(current);
                        break;
                    }
            }
                    
         }
        }
    }

    private void ShowMainMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== HUVUDMENY ===");
        Console.WriteLine("1. Visa saldo");
        Console.WriteLine("2. Ta ut pengar");
        Console.WriteLine("3. Sätt in pengar");
        Console.WriteLine("4. Transaktionshistorik");
        Console.WriteLine("5. Kortinställningar");
        Console.WriteLine("6. Mata ut kort");
        Console.Write("Val: ");

        string? input = Console.ReadLine();

        switch (input)
        {
            case "1":
                ShowBalance(_atmService);
                break;
            case "2":
                WithdrawFlow(_atmService);
                break;
            case "3":
                DepositFlow(_atmService);
                break;
                case "4":
                System.Console.WriteLine("1. Insättningar \n 2. Uttag");
                int VäljaHistorik = Convert.ToInt32(Console.ReadLine());

                if(VäljaHistorik == 1)
                {
                    foreach(string dr in DepositReceipts)
                    {
                        Console.WriteLine(dr);
                    }
                }
                else if(VäljaHistorik == 2)
                {
                foreach(string wr in WithdrawReceipts)
                {
                    Console.WriteLine(wr);
                }
                }
                break;            
            case"5":
            Console.WriteLine("=== HUVUDMENY ===");
            Console.WriteLine("1. Se korttyp");
            Console.WriteLine("2. Ändra pinkod");
            Console.WriteLine("3. Gå tillbaks till huvudmeny");
            string? input1 = Console.ReadLine();

            switch(input1)
                {
                    case "1":
                    string type = _atmService.CheckCardType();
                    Console.WriteLine("Ditt kort är: " + type);
                    break;
                    case "2":
                    Console.WriteLine("Vill du ändra din pinkod? Y/N");
                    string? inputChangePinCode = Console.ReadLine().ToLower();
                    if(inputChangePinCode == "y")
                        {
                            Console.WriteLine("Vänlingen ange din nuvarande pinkod");
                            string inputPinCode = Console.ReadLine();

                            bool ok = _atmService.EnterPin(inputPinCode ?? "");
                            if(ok)
                            {
                                bool running = true;
                                while(running)
                                {
                                Console.WriteLine("Skriv in din nya pinkod");
                                string? PinCodeTry1 = Console.ReadLine();
                                Console.WriteLine("Skriv in din nya pinkod en gång till");
                                string? PinCodeTry2 = Console.ReadLine();
                                if (PinCodeTry1 == PinCodeTry2)
                                    {
                                        _atmService.ChangePin(PinCodeTry2);
                                        running = false;
                                    }
                                else{
                                        System.Console.WriteLine("Koderna du skrev matchade ej varandra.");
                                        System.Console.WriteLine("Vill du gå tillbaks till huvudmenyn? Y/N");
                                        string goBack = Console.ReadLine().ToLower();
                                        if(goBack == "y")
                                        {
                                            running = false;
                                        }
                                    }
                                }
                            }
                        }
                    break;
                    case "3":
                    ShowMainMenu();
                    break;
                }
            break;
            case "6":
                _atmService.EjectCard();
                Console.WriteLine("Kortet är utmatat.");
                break;
            default:
                Console.WriteLine("Ogiltigt val.");
                break;
        }
    }

    private void ShowBalance(AtmService atm)
    {
        int balance = atm.GetBalance();
        Console.WriteLine($"Ditt saldo är: {balance} kr");
    }

    private void WithdrawFlow(AtmService atm)
    {
        Console.Write("Ange belopp att ta ut: ");
        string? input = Console.ReadLine();

        int amount = int.Parse(input);
        
        bool success = atm.Withdraw(amount);

        if (success)
        {
            string receipt = GetWithDrawReceipt(amount);

            WithdrawReceipts.Add(receipt);
            Console.WriteLine("Varsågod, ta dina pengar.");
            Console.WriteLine("Vill du ha ett kvitto på uttaget? Y/N");
            string WantReciept = Console.ReadLine().ToLower();

            if(WantReciept == "y")
            {
                GetWithDrawReceipt(amount);
                Console.WriteLine("Tryck på ENTER för att gå tillbaks.");
                Console.ReadLine();
            }
        }
        else
        {
            Console.WriteLine("Uttaget kunde inte genomföras.");
        }
    }

    private string GetWithDrawReceipt(int amount)
    {
        int currentBalance = _atmService.GetBalance();
        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string receipt = "\n======= UTTAGSKVITTO =======\n" +
                     $"Datum:     {date}\n" +
                     $"Typ:       Kontantuttag\n" +
                     $"Belopp:    -{amount} kr\n" +
                     $"Nytt saldo: {currentBalance} kr\n" +
                     "============================\n";

        return receipt;
    }
    private string GetDepositReceipt(int amount)
    {
        int currentBalance = _atmService.GetBalance();
        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string receipt = "\n======= INSÄTTNINGSKVITTO =======\n" +
                     $"Datum:     {date}\n" +
                     $"Typ:       Kontantinsättning\n" +
                     $"Belopp:    +{amount} kr\n" +
                     $"Nytt saldo: {currentBalance} kr\n" +
                     "============================\n";

        return receipt;
    }
    
    private void DepositFlow(AtmService atm)
    {
        Console.Write("Ange belopp att sätta in: ");
        string? input = Console.ReadLine();

        int amount = int.Parse(input);

        bool success = atm.Deposit(amount);

        if (success)
        {
            string receipt = GetDepositReceipt(amount);

            DepositReceipts.Add(receipt);
           Console.WriteLine("Vill du ha ett kvitto på insättningen? Y/N");
            string WantReciept = Console.ReadLine().ToLower();

            if(WantReciept == "y")
            {
                GetDepositReceipt(amount);
                Console.WriteLine("Tryck på ENTER för att gå tillbaks.");
                Console.ReadLine();
            }
        }
        else
        {
            Console.WriteLine("Insättningen kunde inte genomföras.");
        }
        
    }
    
}