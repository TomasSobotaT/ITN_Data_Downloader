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

        private string pripojovaciString;
        private List<User> seznamITN; 

        public SpravceDB(List<User> seznamITN)
        {
            pripojovaciString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=C:\\GITHUBZALOHA\\DATADOWNLOADER\\DATADOWNLOADER\\DATABASE.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            this.seznamITN = seznamITN;
        }


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

    }
}
