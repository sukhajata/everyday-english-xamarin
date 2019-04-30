using EverydayEnglish3.Data;
using EverydayEnglish3.Services;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

[assembly:Xamarin.Forms.Dependency(typeof(EverydayEnglish3.iOS.Services.ContentManagerImplementation))]
namespace EverydayEnglish3.iOS.Services
{
    public class ContentManagerImplementation : IContentManager
    {
        private SQLiteConnection connection;

        public ContentManagerImplementation()
        {
            connection = GetConnection();
            CreateTables();
        }

        public void CreateTables()
        {
            connection.CreateTable<Lesson>();
            connection.CreateTable<Media>();
            connection.CreateTable<Slide>();
        }

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

        public void DownloadDataFile(string urlString, string fileName)
        {
            var webClient = new WebClient();

            webClient.DownloadDataCompleted += (s, e) => {
                var bytes = e.Result; // get the downloaded data
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string localPath = Path.Combine(documentsPath, fileName);
                File.WriteAllBytes(localPath, bytes); // writes to local storage
            };

            var url = new Uri(urlString);
            webClient.Encoding = Encoding.UTF8;
            webClient.DownloadDataAsync(url);

            //InvokeOnMainThread(() => {

            //});
        }

        public void DownloadTextFile(string urlString, string fileName)
        {
            var webClient = new WebClient();

            webClient.DownloadStringCompleted += (s, e) => {
                var text = e.Result; // get the downloaded text
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string localPath = Path.Combine(documentsPath, fileName);
                File.WriteAllText(localPath, text); // writes to local storage
            };

            var url = new Uri(urlString);
            webClient.Encoding = Encoding.UTF8;
            webClient.DownloadStringAsync(url);

        }

        public void DownloadLessons(Action<string> LessonsDownloadCompleted)
        {
           
                string urlString2 = "http://sukhajata.com/content/lessons.php";
                WebClient webClient2 = new WebClient();
                webClient2.DownloadStringCompleted += (s, e) =>
                {
                    try
                    {
                        var text = e.Result;
                        Lesson[] lessons = JsonConvert.DeserializeObject<Lesson[]>(text);
                        foreach (Lesson lesson in lessons)
                        {
                            connection.Insert(lesson);
                        }

                        LessonsDownloadCompleted.Invoke("");

                    }
                    catch (WebException ex)
                    {
                        LessonsDownloadCompleted(ex.Message);
                    }
                };

                var url2 = new Uri(urlString2);
                webClient2.Encoding = Encoding.UTF8;
                webClient2.DownloadStringAsync(url2);
            
            
        }

        public IEnumerable<Lesson> GetLessons()
        {
            var lessons = connection.Table<Lesson>().Where(c => c.Id > 0);
            return lessons;
        }

        public string GetCategoryInstructions(int catId)
        {
            var query = connection.Table<Category>().Where(c => c.Id == catId);
            return query.First().Instructions;
        }

        public void DownloadLessonMedia(int lessonId, Action<string> MediaDownloadCompleted)
        {

            string urlString2 = "http://sukhajata.com/content/lessonSlides.php?id=" + Convert.ToString(lessonId);
            WebClient webClient2 = new WebClient();
            webClient2.DownloadStringCompleted += (s, e) =>
            {
                try
                {
                    var text = e.Result;
                    Slide[] slides = JsonConvert.DeserializeObject<Slide[]>(text);
                    foreach (Slide slide in slides)
                    {
                        connection.Insert(slide);
                    }
                }
                catch (Exception ex)
                {
                    MediaDownloadCompleted.Invoke(ex.Message);
                }
            };

            var url2 = new Uri(urlString2);
            webClient2.Encoding = Encoding.UTF8;
            webClient2.DownloadStringAsync(url2);


            string urlString = "http://sukhajata.com/content/lessonMedia.php?id=" + Convert.ToString(lessonId);
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += (s, e) => {
                try
                {
                    var text = e.Result; // get the downloaded text
                    Media[] medias = JsonConvert.DeserializeObject<Media[]>(text);
                    foreach (Media m in medias)
                    {
                        connection.Insert(m);
                        if (m.ImageFileName != null && m.ImageFileName.Length > 0)
                        {
                            string imageUrl = "http://sukhajata.com/images/" + m.ImageFileName;
                            DownloadDataFile(imageUrl, m.ImageFileName);
                        }
                        if (m.AudioFileName != null && m.AudioFileName.Length > 0)
                        {

                        }
                    }

                    MediaDownloadCompleted.Invoke("");
                }
                catch (Exception ex)
                {
                    MediaDownloadCompleted.Invoke(ex.Message);
                }
            };

            var url = new Uri(urlString);
            webClient.Encoding = Encoding.UTF8;
            webClient.DownloadStringAsync(url);


        }

        public void DownloadSlides(int lessonId, Action<string> SlideDownloadCompleted)
        {
            string urlString = "http://sukhajata.com/content/slides.php?id=" + Convert.ToString(lessonId);
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += (s, e) => {
                try
                {
                    var text = e.Result; // get the downloaded text
                    SlideWithMedia[] slides = JsonConvert.DeserializeObject<SlideWithMedia[]>(text);
                    foreach (SlideWithMedia slide in slides)
                    {
                        Slide ss = new Slide();
                        ss.Id = slide.Id;
                        ss.LessonId = lessonId;
                        ss.CategoryId = slide.CategoryId;
                        ss.Content = slide.Content;
                        connection.Insert(ss);

                        foreach (Media media in slide.Medias)
                        {
                            connection.Insert(media);
                        }

                    }

                    SlideDownloadCompleted.Invoke("");
                }
                catch (Exception ex)
                {
                    SlideDownloadCompleted.Invoke(ex.Message);
                }
            };

            var url = new Uri(urlString);
            webClient.Encoding = Encoding.UTF8;
            webClient.DownloadStringAsync(url);
        }

        public IEnumerable<Slide> GetSlides(int lessonId)
        {
            var slides = connection.Table<Slide>().Where(s => s.LessonId == lessonId);
            return slides;
        }


    }
}