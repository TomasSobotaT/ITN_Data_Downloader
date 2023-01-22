using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDownloader
{
    internal class SpravceDB
    {
        //connectionstring
        private string pripojovaciString;

        //seznam uzivatelu stahnutý třídou ITNetwork
        private List<User> seznamITN;

        public SpravceDB(List<User> seznamITN)
        {
            string adresar = Directory.GetCurrentDirectory();
            pripojovaciString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + adresar + "\\Database.mdf;Integrated Security=True";
            this.seznamITN = seznamITN;
        }


        /// <summary>
        /// Metoda připojí program k databázi a uloží stažené hodnoty z ITnetwork
        /// </summary>
        public void ZpracujData()
        {
            using (SqlConnection spojeni = new SqlConnection(pripojovaciString))
            {

                spojeni.Open();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Aplikace se úspěšně připojila k databázi.");


                using (SqlCommand prikaz = new SqlCommand()) {
                    prikaz.Connection = spojeni;

                    prikaz.CommandText = "INSERT INTO" +
                                              " ITN_USERS(Jmeno,UrlObrazek,Urlwww,IdNaITN,Vek,Zkusenost,Aura)" +
                                              " VALUES " +
                                              " (@Jmeno,@UrlObrazek,@Urlwww,@IdNaITN,@Vek,@Zkusenost,@Aura)";

                    for (int i = 0;i<seznamITN.Count;i++)
                    {
                        prikaz.Parameters.Clear();

                        prikaz.Parameters.AddWithValue("@Jmeno", seznamITN[i].Jmeno);
                        prikaz.Parameters.AddWithValue("@UrlObrazek", seznamITN[i].ObrazekWWW);
                        prikaz.Parameters.AddWithValue("@Urlwww", seznamITN[i].Stranka);
                        prikaz.Parameters.AddWithValue("@IdNaITN", seznamITN[i].Id);
                        prikaz.Parameters.AddWithValue("@Vek", seznamITN[i].Vek);
                        prikaz.Parameters.AddWithValue("@Zkusenost", seznamITN[i].Zkusenost);
                        prikaz.Parameters.AddWithValue("@Aura", seznamITN[i].Aura);

                       prikaz.ExecuteNonQuery();
                      
                    }

                    

            }
        }
           
        
        
        }


        public int VratPosledniId()
        {
            using (SqlConnection spojeni = new SqlConnection(pripojovaciString))
            {

                spojeni.Open();
               


                using (SqlCommand prikaz = new SqlCommand())
                {
                    prikaz.Connection = spojeni;

                    prikaz.CommandText = "SELECT MAX([IdNaITN]) FROM [ITN_USERS]";

                   var result = prikaz.ExecuteScalar();

                    if (result == System.DBNull.Value)
                    {
                        return 0;
                    }
                    else
                    {
                        return (int)result;
                    }
                        
                    

                }
            }
        }

    }
}
