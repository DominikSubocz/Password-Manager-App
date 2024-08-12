using MySql.Data.MySqlClient;
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
        const string M_str_sqlcon = "Server=localhost;Database=passlockdb;Uid=root;";

        public void createDB()
        {
            using (var mySqlCn = new MySqlConnection(M_str_sqlcon))
            {
                mySqlCn.Open();
                using (var mySqlCmd = new MySqlCommand("CREATE DATABASE IF NOT EXISTS passlockdb", mySqlCn))
                {
                    Debug.WriteLine("Done!");
                }

                mySqlCn.Close();

            }

        }

        public void createDbTables()
        {

        }
    }
}
