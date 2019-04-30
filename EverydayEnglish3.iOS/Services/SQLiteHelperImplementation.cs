using EverydayEnglish3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using SQLite;
using System.IO;

[assembly:Dependency (typeof(EverydayEnglish3.iOS.Services.SQLiteHelperImplementation))]
namespace EverydayEnglish3.iOS.Services
{
    public class SQLiteHelperImplementation : ISQLiteHelper
    {
        public SQLiteHelperImplementation() { }

        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "EverydayEnglishSQLite.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);

            if (!File.Exists(path))
            {
                File.Create(path);
            }

            // Create the connection
            var conn = new SQLite.SQLiteConnection(path);
            // Return the database connection
            return conn;
        }
    }
}