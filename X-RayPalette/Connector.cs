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
        public MySqlCommand Cmd;
        public Connector() 
        {
            string dataBase = "server=81.171.31.232;user=RTG_ordynator;database=pracowaniartg;port=3306;password=RTG_ordynator1;";
            MySqlConnection conn = new MySqlConnection(dataBase);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                Console.WriteLine("Connected to MySQL.");
                Cmd = new MySqlCommand();
                Cmd.Connection = conn;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
            }
        }       
    }
}



