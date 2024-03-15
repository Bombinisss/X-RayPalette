using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace X_RayPalette
{
    public class Connector
    {
        public Connector() 
        {
            string DataBase = "server=localhost;user=root;database=inz_opr_med_app;port=3306;password=Admin12;";
            MySqlConnection conn = new MySqlConnection(DataBase);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                Console.WriteLine("Connected to MySQL.");
            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
            }
        }       
    }
}



