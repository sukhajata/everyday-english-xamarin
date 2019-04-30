using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using EverydayEnglish3.Services;
using SQLite;
using System.IO;
using EverydayEnglish3.Data;

[assembly : Dependency (typeof(EverydayEnglish3.Droid.Services.SQLiteHelperImplementation))]
namespace EverydayEnglish3.Droid.Services
{

    public class SQLiteHelperImplementation : ISQLiteHelper
    {
        private SQLiteConnection connection;

        public SQLiteHelperImplementation()
        {
            connection = GetConnection();
            CreateTables();
        }

        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "EverydayEnglishSQLite.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);

            if (!File.Exists(path))
            {
                File.Create(path);
            }

            // Create the connection
            var conn = new SQLite.SQLiteConnection(path);
            // Return the database connection
            return conn;
        }

        public string GetCategoryInstructions(int catId)
        {
            var query = connection.Table<Category>().Where(c => c.Id == catId);
            return query.First().Instructions;
        }

        public IEnumerable<Lesson> GetLessons()
        {
            var lessons = connection.Table<Lesson>().Where(c => c.Id > 0);
            return lessons;
        }

        public void CreateTables()
        {
            connection.CreateTable<Media>();
            connection.CreateTable<Slide>();
            connection.CreateTable<Lesson>();
        }
    }
}