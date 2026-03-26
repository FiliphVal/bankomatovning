
using ATM;

// 1. Skapa tjänsten först
AtmService atm = new AtmService(10000);

// 2. Skapa ett OBJEKT (en instans) av ConsoleRunner
// Här körs konstruktorn som fyller din 'cards'-lista!
ConsoleRunner runner = new ConsoleRunner(atm); 

// 3. Anropa Run på objektet 'runner' istället för på klassen
runner.Run();

