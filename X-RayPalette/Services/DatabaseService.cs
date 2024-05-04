using MySql.Data.MySqlClient;
using System.Dynamic;

namespace X_RayPalette.Services
{
    public class DatabaseService
    {
        private string _hostname;
        private string _username;
        private string _password;
        private string _database;
        private int _port;
        bool _isConnected = false;
        private MySqlConnection _connection;
        public DatabaseService(string hostname,string database, string username, string password,int port, bool autoConnect=true)
        {
            _hostname = hostname;
            _username = username;
            _password = password;
            _database = database;
            _port = port;
            if (autoConnect)
                Connect();
        }
        public bool Connect()
        {
            if (_isConnected)
                return true;
     
            string connectionsString = $"server={_hostname};user={_username};database={_database};port={_port.ToString()};password={_password};";
            _connection = new MySqlConnection(connectionsString);
            try
            {
                _connection.Open();
                Console.WriteLine($"Connected to Db ({_database})");
                _isConnected = true;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
                _isConnected = false;
            }
            return _isConnected;
        }
        public bool Disconnect()
        {
            if (!_isConnected)
                return true;
            try
            {
                _connection.Close();
                _isConnected = false;
                Console.WriteLine($"Disconnected from Db ({_database})");
            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
                _isConnected = true;
            }
            return _isConnected;
        }
        
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
        }

        //base metods for more complex queries
        public MySqlDataReader ExecuteFromSql(string sql, params object[] parameters)
        {
            if (!_isConnected)
                return null;

            MySqlCommand cmd = new MySqlCommand(sql, _connection);

            for (int i = 0; i < parameters.Length; i++)
            {
                cmd.Parameters.AddWithValue($"@p{i}", parameters[i]);
            }

            return cmd.ExecuteReader();
        }
        public int ExecuteNonQuery(string sql, params object[] parameters)
        {
            if (!_isConnected)
                return -1;

            MySqlCommand cmd = new MySqlCommand(sql, _connection);
            

            for (int i = 0; i < parameters.Length; i++)
            {
                cmd.Parameters.AddWithValue($"@p{i}", parameters[i]);
            }

            return cmd.ExecuteNonQuery();
        }
        public dynamic ExecuteSelect(string sql, params object[] parameters)
        {
            if (!_isConnected)
                return null;

            using (MySqlCommand cmd = new MySqlCommand(sql, _connection))
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    cmd.Parameters.AddWithValue($"@p{i}", parameters[i]);
                }

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    var result = new List<dynamic>();
                    while (reader.Read())
                    {
                        dynamic row = new ExpandoObject();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            ((IDictionary<string, object>)row)[reader.GetName(i)] = reader.GetValue(i);
                        }
                        result.Add(row);
                    }
                    return result;
                }
            }
        }
        
        public object ExecuteScalar(string sql, params object[] parameters)
        {
            if (!_isConnected)
                return null;

            MySqlCommand cmd = new MySqlCommand(sql, _connection);

            for (int i = 0; i < parameters.Length; i++)
            {
                cmd.Parameters.AddWithValue($"@p{i}", parameters[i]);
            }

            return cmd.ExecuteScalar();
        }

        public void Dispose()
        {
            if (_isConnected)
                Disconnect();
        }

        //methods for more specific use cases
        public string AuthUser(string login, string password)
        {
            if (!_isConnected)
                return null;

            string sql = "SELECT login, password FROM login_info WHERE login = @p0";
            using (MySqlDataReader reader = ExecuteFromSql(sql, login))
            {
                if (reader.Read())
                {
                    string hashedPassword = reader.GetString(1);
                    if (BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword))
                    {
                        var res= reader.GetString(0);
                        reader.Close();
                        return res;
                    }
                }
            }
            return null;
        }
        
    }
}
