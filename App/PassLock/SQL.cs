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

        public void connect()
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
                if (connection != null)
                {
                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: Could not connect to datbase: " + ex.ToString());
            }
        }

        public void disconnect()
        {
            connection.Close();
        }

        public async void createDB()
        {
            try
            {
                connect();
                cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SHOW DATABASES LIKE 'passlockdb'";
                cmd.ExecuteNonQuery();
                string result = (string)cmd.ExecuteScalar();
                Debug.WriteLine("Checking if database already exists!");
                Debug.WriteLine(result);

                if (result == null)
                {
                    // Create tables
                    Debug.WriteLine("Database does not exist. Creating new database!");
                    StringBuilder sqlBuilder = new StringBuilder();
                    sqlBuilder.Append("CREATE DATABASE passlockdb; USE passlockdb;");
                    sqlBuilder.AppendLine("CREATE TABLE Users (UserID INT AUTO_INCREMENT PRIMARY KEY, Username VARCHAR(255) NOT NULL UNIQUE, PasswordHash VARCHAR(255) NOT NULL, Email VARCHAR(255) NOT NULL UNIQUE, CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP, LastLogin TIMESTAMP NULL);");
                    sqlBuilder.AppendLine("CREATE TABLE Passwords (PasswordID INT AUTO_INCREMENT PRIMARY KEY, UserID INT NOT NULL, ServiceName VARCHAR(255) NOT NULL, Username VARCHAR(255) NOT NULL, PasswordHash VARCHAR(255) NOT NULL, CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP, UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP, FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE);");
                    sqlBuilder.AppendLine("CREATE TABLE Documents (DocumentID INT AUTO_INCREMENT PRIMARY KEY, UserID INT NOT NULL, DocumentName VARCHAR(255) NOT NULL, DocumentType VARCHAR(100) NOT NULL, DocumentPath TEXT NOT NULL, CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP, FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE);");
                    sqlBuilder.AppendLine("CREATE TABLE Tags (TagID INT AUTO_INCREMENT PRIMARY KEY, TagName VARCHAR(100) NOT NULL UNIQUE);");
                    sqlBuilder.AppendLine("CREATE TABLE PasswordTags (PasswordTagID INT AUTO_INCREMENT PRIMARY KEY, PasswordID INT NOT NULL, TagID INT NOT NULL, FOREIGN KEY (PasswordID) REFERENCES Passwords(PasswordID) ON DELETE CASCADE, FOREIGN KEY (TagID) REFERENCES Tags(TagID) ON DELETE CASCADE);");
                    sqlBuilder.AppendLine("CREATE TABLE DocumentTags (DocumentTagID INT AUTO_INCREMENT PRIMARY KEY, DocumentID INT NOT NULL, TagID INT NOT NULL, FOREIGN KEY (DocumentID) REFERENCES Documents(DocumentID) ON DELETE CASCADE, FOREIGN KEY (TagID) REFERENCES Tags(TagID) ON DELETE CASCADE);");

                    cmd.CommandText = sqlBuilder.ToString();
                    cmd.ExecuteNonQuery();
                    Debug.WriteLine("Database created successfully!");

                }
                else
                {
                    Debug.WriteLine("Database already exists! Application ready to run");

                }

                disconnect();

            }
            catch (Exception x)
            {
                Debug.WriteLine("Error running command!" + " " + x);
            }
        }

        public bool usersExist()
        {
            bool usersExist = false;
            try
            {
                connect();
                cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "USE passlockdb; SELECT * FROM users;";
                cmd.ExecuteNonQuery();
                string result = (string)cmd.ExecuteScalar();
                if(result == null)
                {
                    Debug.WriteLine("No users found! Please create an account to continue!");
                    usersExist = false;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error running command!" + " " + ex);
            }
            return usersExist;
        }
    }


}


