using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataDownloader
{
    /// <summary>
    /// Třída stahující data o uživatelích z ITNetwork.cz
    /// </summary>
    internal class ITNetwork
    {

        //celé url na uživatele - https://www.itnetwork.cz/portfolio/" + ID
        public string CelaStranka { get; private set; }

        //string základu stránky (vždy stejný)
        public string StrankaVzor { get; private set; }

        //Id aktuálně zpracovavaného uživatele
        public string Id { get; private set; }

        //seznam stahnutých uživatelů
        public List<User> seznam { get; private set; }

        private HttpClient klient;

        //public string Stranka { get; private set; }
        //public string VzorovaPrazdnaStranka { get; private set; } = "";



        /// <summary>
        /// Konstruktor třídy ITNetwork
        /// </summary>
        public ITNetwork() 
        {
            klient = new HttpClient();
            seznam = new List<User>();
            StrankaVzor = "https://www.itnetwork.cz/portfolio/";
        } 

       


        /// <summary>
        /// Metoda začínající stahování dat uživatelů, pokud je url validní, předá její html kod metode VytahniData() a vrácený objekt User přidá do seznamu
        /// </summary>
        /// <param name="a">od jakého Id začít stahovat</param>
        /// <param name="b">do jakého Id (včetně) se bude stahovat </param>
        public async void Stahuj(int a, int b)
        {
         


            for (int i = a; i < b+1; i++)
            {

                Id = i.ToString();

                if (UrlIsValid(StrankaVzor + Id))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(Id + " - UŽIVATEL EXISTUJE");
                    CelaStranka = klient.GetStringAsync(StrankaVzor + Id).Result;
                    seznam.Add(VytahniData(CelaStranka, i));
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(Id + " - UŽIVATEL NEEXISTUJE");
               
                }


            }

        }


        /// <summary>
        /// Metoda která najde konkrétní data v HTML kodu stránky a vráti instanci třídy User
        /// </summary>
        /// <param name="text">HTML kod stranky</param>
        /// <param name="i">Id uživatele</param>
        /// <returns></returns>
        public User VytahniData(string text, int i)
        {

            using (StringReader reader = new StringReader(text))
            {
                string radek = "";              //aktuálně zpracovávaný řádek text v HTML kodu

                string radekswww = "";          //radek obsahující url www stránky uživatele, pokud ji má
                string radeksejmenem = "";      //radek obsahující jmeno/nick uživatele
                string radekfoto = "";          //radek obsahující url fota uživatele
                string radeksezkusenosti = "";  //radek obsahující zkušenostuživatele
                string radeksaurou = "";        //radek obsahující auru uživatele
                string radeksvekem = "";        //radek obsahujcí vek, pokud ho uživatel zadal

                string jmeno = "";              //jméno/nick uživatele
                string foto = "";               //url fota uživatele, foto neukládáme, jen odkaz na něj
                string www = "";                //url www stránky uživatele, pokud ji má
                int zkusenost = 0;              // zkusenost uživatele
                int aura = 0;                   // aura uživatele
                int vek = -1;                   //věk uživatele, pokud nemá zadaný = -1

                // hledání řádku s příslušnou hodnotou
                while ((radek = reader.ReadLine()) != null)
                {
                    if (radek.Contains("<title>"))
                    {
                        radeksejmenem = radek.Trim();
                    }
                    if (radek.Contains("member-card circle thumbnail-128"))
                    {
                        radekfoto = radek.Trim();
                    }
                    if (radek.Contains("target=\"_blank\" rel=\"nofollow\""))
                    {
                        radekswww = radek.Trim();
                    }
                    if (radek.Contains("Zkušeností"))
                    {
                        radeksezkusenosti= radek.Trim();
                    }
                    if (radek.Contains("<br><br></span>") && !radek.Contains("let"))
                    {
                        radeksaurou = radek.Trim();
                    }
                    if (radek.Contains("let<br><br></span>"))
                    {
                        radeksvekem = radek.Trim();
                    }
                }

                //extrahování hodnot z řádků

                jmeno = radeksejmenem.Replace("<title>", "").Replace("</title>", "");

                foto = radekfoto.Replace($"<a href=\"portfolio/{i}\"><img src=\"", "");
                //foto = foto.Replace("\" alt=\"Avatar\" data-member-id=\"{i}\" class=\"member-card circle thumbnail-128\" /></a>", "");
                int alt = foto.LastIndexOf("alt");
                foto = foto.Substring(0, alt - 2);
                foto = "https://www.itnetwork.cz/" + foto;

                www = radekswww.Replace("\" target=\"_blank\" rel=\"nofollow\">", "").Replace("<a href=\"", "");

                if (radeksvekem != "")
                {
                    radeksvekem  = radeksvekem.Replace(" let<br><br></span>", "").Replace("<span>", "");
                    var dotaz = radeksvekem.Where(char.IsDigit).ToArray();
                    vek = int.Parse(new string(dotaz));
                }

                int index = radeksezkusenosti.IndexOf("/");
                string stringsezkusenosti = radeksezkusenosti.Substring(0, index);
                var dotaz2 =  stringsezkusenosti.Where(char.IsDigit).ToArray();
                zkusenost = int.Parse(new string(dotaz2));


                aura = int.Parse(radeksaurou.Replace("<br><br></span>","").Replace("<span>",""));


                //vrácení hotové instance třídy User
                return new User(jmeno, foto, www, i, vek,zkusenost, aura);
            }





        }

        /// <summary>
        /// Metoda která kontroluje jestli je url platná
        /// </summary>
        /// <param name="url">www adresa</param>
        /// <returns></returns>
        //ZDROJ: https://stackoverflow.com/questions/924679/c-sharp-how-can-i-check-if-a-url-exists-is-valid
        public bool UrlIsValid(string url)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 5000; //set the timeout to 5 seconds to keep the user from waiting too long for the page to load
                request.Method = "HEAD"; //Get only the header information -- no need to download any content

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    int statusCode = (int)response.StatusCode;
                    if (statusCode >= 100 && statusCode < 400) //Good requests
                    {
                        return true;
                    }
                    else if (statusCode >= 500 && statusCode <= 510) //Server Errors
                    {
                        //log.Warn(String.Format("The remote server has thrown an internal error. Url is not valid: {0}", url));
                        Debug.WriteLine(String.Format("The remote server has thrown an internal error. Url is not valid: {0}", url));
                        return false;
                    }
                }
            }
            catch (WebException ex)
            {

                return false;

            }
            catch (Exception ex)
            {

            }
            return false;
        }


    }
}
