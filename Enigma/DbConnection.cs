using System;
using System.IO;
using System.Data.SQLite;

namespace Enigma
{
    class DbConnection
    {
        private string _path =  Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Enigma\";
        private SQLiteConnection _db;

        public DbConnection()
        {
            if (!File.Exists(_path + "Enigma.db"))
            {
                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                    AppDomain.CurrentDomain.SetData("DataDirectory", _path);
                    _db = new SQLiteConnection("Data Source=|DataDirectory|Enigma.db");
                    SetupDB();
                } else {
                    AppDomain.CurrentDomain.SetData("DataDirectory", _path);
                    _db = new SQLiteConnection("Data Source=|DataDirectory|Enigma.db");
                }
            } 
        }

        private void SetupDB()
        {
            string sql = @"CREATE TABLE secrets (
                            Id INTEGER PRIMARY KEY,
                            UserId INTEGER NOT NULL,
                            Name TEXT NOT NULL UNIQUE,
                            Username TEXT,
                            Password TEXT,
                            Remarks TEXT
                            )";
            _db.Open();
            SQLiteCommand command = new SQLiteCommand(sql, _db);
            command.ExecuteNonQuery();
            _db.Close();
        }

    }
}
