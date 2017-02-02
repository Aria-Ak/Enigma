using System;
using System.IO;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Enigma
{
    class DbConnection
    {
        private string _path =  Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Enigma\";
        private SQLiteConnection _db;
        private static DbConnection instance = new DbConnection();
        


        private DbConnection()
        {
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);

            AppDomain.CurrentDomain.SetData("DataDirectory", _path);
            _db = new SQLiteConnection("Data Source=|DataDirectory|Enigma.db");

            if (!File.Exists(_path + "Enigma.db"))
                SetupDB();
        }

        public static DbConnection getInstance()
        {
            return instance;
        }

        private void SetupDB()
        {
            string createSecrets = @"CREATE TABLE Secrets (
                            Id INTEGER PRIMARY KEY,
                            UserId INTEGER NOT NULL,
                            Name TEXT NOT NULL UNIQUE,
                            Username TEXT,
                            Password TEXT,
                            Remarks TEXT
                            )";

            string createUsers = @"CREATE TABLE Users (
                            Id INTEGER PRIMARY KEY,
                            Username TEXT,
                            Password TEXT
                            )";

            string createConfig = @"CREATE TABLE Configs (
                            Id INTEGER PRIMARY KEY,
                            LastUsername TEXT,
                            LastPassword TEXT,
                            RememberMe INTEGER,
                            StayLoggedIn INTEGER
                            )";


            string InsertConfig = @"INSERT INTO Configs (Id,LastUsername,LastPassword ,RememberMe, StayLoggedIn)
                                    VALUES 
                                    (0,'','',0,0)";

            _db.Open();
            new SQLiteCommand(createSecrets, _db).ExecuteNonQuery();
            new SQLiteCommand(createUsers, _db).ExecuteNonQuery();
            new SQLiteCommand(createConfig, _db).ExecuteNonQuery();
            new SQLiteCommand(InsertConfig, _db).ExecuteNonQuery();
            _db.Close();
        }

        public bool IsUsernameExist(string name)
        {
            bool result = false;
            string getUsername = "SELECT * FROM Users where Username=@name";
            SQLiteCommand command = new SQLiteCommand(getUsername, _db);
            command.Parameters.AddWithValue("@name", name);
            _db.Open();
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                result = true;
            }
            reader.Close();
            _db.Close();
            return result;
        }


        public void InsertUser(string user, string pass)
        {

            string getUsername = @"INSERT INTO Users
                                    (Id, Username,Password)
                                    Values 
                                    (NULL,@user,@pass)";

            SQLiteCommand command = new SQLiteCommand(getUsername, _db);
            command.Parameters.AddWithValue("@user", user);
            command.Parameters.AddWithValue("@pass", pass);
            _db.Open();
            command.ExecuteNonQuery();
            _db.Close();
        }


        public int GetUserIdByName(string name)
        {
            int result = 0;
            string getUsername = "SELECT Id FROM Users where Username=@name";
            SQLiteCommand command = new SQLiteCommand(getUsername, _db);
            command.Parameters.AddWithValue("@name", name);
            _db.Open();
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                result = Convert.ToInt32(reader["Id"]);
            }
            reader.Close();
            _db.Close();
            return result;
        }

        public void InsertConfig(Config config)
        {
            try
            {
                string sql = @"UPDATE Configs
                                SET
                                Id = @Id,
                                LastUsername = @LastUsername,
                                LastPassword = @LastPassword,
                                RememberMe = @RememberMe,
                                StayLoggedIn = @StayLoggedIn
                                WHERE Id = 0";

                SQLiteCommand command = new SQLiteCommand(sql, _db);
                command.Parameters.AddWithValue("@Id", 0);
                command.Parameters.AddWithValue("@LastUsername", config.LastUsername);
                command.Parameters.AddWithValue("@LastPassword", config.LastPassword);
                command.Parameters.AddWithValue("@RememberMe", Convert.ToInt32(config.RememberMe));
                command.Parameters.AddWithValue("@StayLoggedIn", Convert.ToInt32(config.StayLoggedIn));

                _db.Open();
                command.ExecuteNonQuery();
                _db.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertSecret(Secret item)
        {
            try
            {
                string sql = @"INSERT INTO Secrets
                                    (Id, UserId,Name,Username,Password,Remarks)
                                    Values 
                                    (NULL,@userid,@name,@username,@password,@remarks)";

                SQLiteCommand command = new SQLiteCommand(sql, _db);
                command.Parameters.AddWithValue("@userid", item.UserId);
                command.Parameters.AddWithValue("@name", item.Name);
                command.Parameters.AddWithValue("@username", item.Username);
                command.Parameters.AddWithValue("@password", item.Password);
                command.Parameters.AddWithValue("@remarks", item.Remarks);

                _db.Open();
                command.ExecuteNonQuery();
                _db.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<Secret> GetSecrets(int UserId)
        {
            try
            {
                List<Secret> list = new List<Secret>();
                
                string sql = "SELECT * FROM Secrets where UserId=@UserId";
                SQLiteCommand command = new SQLiteCommand(sql, _db);
                command.Parameters.AddWithValue("@UserId", UserId);
                _db.Open();
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var item = new Secret();

                    item.Name = reader["Name"].ToString();
                    item.Username = reader["Username"].ToString();
                    item.Password = reader["Password"].ToString();
                    item.Remarks = reader["Remarks"].ToString();

                    list.Add(item);

                }
                reader.Close();
                _db.Close();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Config GetConfig()
        {
            try
            { 
                var item = new Config();
                string sql = "SELECT * FROM Configs where Id=0";
                SQLiteCommand command = new SQLiteCommand(sql, _db);
                _db.Open();
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    item.LastUsername = reader["LastUsername"].ToString();
                    item.LastPassword = reader["LastPassword"].ToString();
                    item.RememberMe = Convert.ToBoolean(reader["RememberMe"]);
                    item.StayLoggedIn = Convert.ToBoolean(reader["StayLoggedIn"]);
                }
                reader.Close();
                _db.Close();
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool verifyPassword(int userid,string password)
        {
            try
            {
                bool result = false;
                string sql = "SELECT Password FROM Users where Id=@userid";
                SQLiteCommand command = new SQLiteCommand(sql, _db);
                command.Parameters.AddWithValue("@UserId", userid);
                _db.Open();
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (password == reader["Password"].ToString())
                        result = true;
                }
                reader.Close();
                _db.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
