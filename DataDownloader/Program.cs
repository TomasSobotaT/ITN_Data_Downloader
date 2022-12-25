using DataDownloader;


//Vypíše základní menu
Console.SetWindowSize(100, Console.WindowHeight);
Console.WriteLine("\r\n  _____        _          _____                      _                 _             __   ___  \r\n |  __ \\      | |        |  __ \\                    | |               | |           /_ | / _ \\ \r\n | |  | | __ _| |_ __ _  | |  | | _____      ___ __ | | ___   __ _  __| | ___ _ __   | || | | |\r\n | |  | |/ _` | __/ _` | | |  | |/ _ \\ \\ /\\ / / '_ \\| |/ _ \\ / _` |/ _` |/ _ \\ '__|  | || | | |\r\n | |__| | (_| | || (_| | | |__| | (_) \\ V  V /| | | | | (_) | (_| | (_| |  __/ |     | || |_| |\r\n |_____/ \\__,_|\\__\\__,_| |_____/ \\___/ \\_/\\_/ |_| |_|_|\\___/ \\__,_|\\__,_|\\___|_|     |_(_)___/ \r\n                                                                                               \r\n                                                                                               \r\n");
Console.WriteLine();
Console.Write("Formát stahování: |https://www.itnetwork.cz/portfolio/");
Console.ForegroundColor = ConsoleColor.Green;
Console.Write("ID");
Console.ResetColor();
Console.WriteLine("|");
Console.WriteLine();

ITNetwork stahovacIT = new ITNetwork();
int x = 0;
int y = 100;

//zadávání hodnot
Console.Write("Zadejte číslo od kterého se bude ID stahovat (Defaultně: 0): ");
while (!int.TryParse(Console.ReadLine(), out x))
{
    Console.Write("Zadej platné číslo:");
}
Console.Write("Zadejte číslo do kterého se bude ID stahovat (Defaultně: 100): ");
while (!int.TryParse(Console.ReadLine(), out y))
{
    Console.Write("Zadej platné číslo:");
}


Console.WriteLine();

//zapnutí měření času
DateTime start = DateTime.Now;

//spuštění stahování uživatelů
try
{
    stahovacIT.Stahuj(x, y);
}
catch (Exception ex)
{
    Console.WriteLine("Nastala chyba při stahování dat z ITNetwork.cz: " + ex.Message);
}


Console.ResetColor();

// zapnutí zasání do databáze
try
{
    SpravceDB spravceDatabaze = new SpravceDB(stahovacIT.seznam);
    spravceDatabaze.ZpracujData();
    Console.WriteLine("Data úspěšně zapsána do databáze.");
}
catch (Exception ex)
{
    Console.WriteLine("Nastala chyba při práci s databází: "+ex.Message);
}


Console.ResetColor();
DateTime ted = DateTime.Now;
TimeSpan cas = ted - start;
Console.WriteLine("HOTOVO za " + String.Format("{0} minut a {1} vteřin", cas.Minutes, cas.Seconds) ); 
Console.ReadKey();





// testovací kod pro ukladání do csv
//using (StreamWriter sw = new StreamWriter("data.txt"))
//{

//    foreach (var item in stahovacIT.seznam)
//    {
//        sw.WriteLine(item.Jmeno + ";" + item.ObrazekWWW + ";" + item.Stranka + ";" + item.Id + ";" +
//            item.Vek + ";" + item.Zkusenost + ";" + item.Aura);

//    }

//    sw.Close();
//}