using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassLock
{
    internal class SQL
    {

        private static MySqlConnection connection;
        private static MySqlCommand cmd = null;
        private static DataTable dt;
        private static MySqlDataAdapter sda;

        public async void createDB()
        {


            try
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = "127.0.0.1";
                builder.UserID = "root";
                builder.Password = "root%Password";
                builder.SslMode = MySqlSslMode.None;
                connection = new MySqlConnection(builder.ToString());
                Debug.WriteLine("Connected successfully!");

                try
                {
                    if(connection != null)
                    {
                        connection.Open();
                        cmd = connection.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SHOW DATABASES LIKE 'passlockdb'";
                        cmd.ExecuteNonQuery();
                        string result = (string)cmd.ExecuteScalar();
                        if(result == null) {
                            // Create tables
                            StringBuilder sqlBuilder = new StringBuilder();
                            sqlBuilder.AppendLine();


                        }
                        Debug.WriteLine("hello!");
                        Debug.WriteLine(result);
                        connection.Close();

                    }
                } catch (Exception x)
                {
                    connection.Close();
                    Debug.WriteLine("Error running command!");
                }
            } catch (Exception ex)
            {
                Debug.WriteLine("connection failed");
            }

        }

        public void createDbTables()
        {

        }
    }
}
