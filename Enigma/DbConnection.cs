using System;
using System.IO;
using System.Data.SQLite;

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
            _db.Open();
            new SQLiteCommand(createSecrets, _db).ExecuteNonQuery();
            new SQLiteCommand(createUsers, _db).ExecuteNonQuery();
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
            _db.Close();

            return result;
        }


        public void CreateUser(string user, string pass)
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
            _db.Close();
            return result;
        }




    }
}
