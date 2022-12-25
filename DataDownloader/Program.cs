using DataDownloader;



Console.WriteLine("-----------------------------------");
Console.WriteLine("Data Downloader 1.0");
Console.WriteLine("-----------------------------------");
Console.WriteLine();
Console.WriteLine("Formát stahování: |https://www.itnetwork.cz/portfolio/ID|");
Console.WriteLine();
ITNetwork stahovacIT = new ITNetwork();
Console.Write("Zadejte číslo od kterého se bude ID stahovat: (Defaultně: 0) ");
int x = int.Parse(Console.ReadLine());
Console.Write("Zadejte číslo do kterého se bude ID stahovat: (Defaultně: 100000)");
int y = int.Parse(Console.ReadLine());
Console.WriteLine();
DateTime start = DateTime.Now;

stahovacIT.Stahuj(x, y);
Console.ResetColor();

SpravceDB spravceDatabaze = new SpravceDB(stahovacIT.seznam);



spravceDatabaze.ZpracujData();

DateTime ted = DateTime.Now;
TimeSpan cas = ted - start;
Console.WriteLine("HOTOVO za " + cas.TotalSeconds+ " sekund.");
Console.WriteLine();






//using (StreamWriter sw = new StreamWriter("data.txt"))
//{

//    foreach (var item in stahovacIT.seznam)
//    {
//        sw.WriteLine(item.Jmeno + ";" + item.ObrazekWWW + ";" + item.Stranka + ";" + item.Id + ";" +
//            item.Vek + ";" + item.Zkusenost + ";" + item.Aura);

//    }

//    sw.Close();
//}