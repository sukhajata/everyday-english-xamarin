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
using EverydayEnglish3.Services;
using Java.IO;
using Java.Net;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using EverydayEnglish3.Data;
using SQLite;
using Xamarin.Forms;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(EverydayEnglish3.Droid.Services.ContentManagerImplementation))]
namespace EverydayEnglish3.Droid.Services
{
    public class ContentManagerImplementation : IContentManager
    {
        private SQLiteConnection connection;
        private Action SyncCompleted;
        private Action<string> ShowError;
        private int numLessonsToCache;

        public ContentManagerImplementation()
        {
            connection = GetConnection();

           

        }

        public void SyncData(int _numLessonsToCache, Action _SyncCompleted, Action<string> _ShowError)
        {
            SyncCompleted = _SyncCompleted;
            ShowError = _ShowError;
            numLessonsToCache = _numLessonsToCache;

            //create tables if they don't exist
            CreateTables();

            //check for categories
            if (connection.Table<Category>().Count() == 0)
            {
                DownloadCategories(SyncLessons);
            }
            else
            {
                SyncLessons();
            }
            
        }

        private void SyncLessons()
        {
            //download new lessons
            int lastOrder = 0;
            if (connection.Table<Lesson>().Count() > 0)
            {
                var result = connection.Table<Lesson>().OrderByDescending(l => l.LessonOrder).Take<Lesson>(1);
                lastOrder = result.First().LessonOrder;
            }
            
            
            DownloadLessons(lastOrder);
        }

        //private void LessonDownloadCompleted(int lastOrder)
        //{
        //    //download new slides
        //    //var result = connection.Query<int>("SELECT DISTINCT LessonId FROM Slide ORDER BY LessonId DESC LIMIT 1");
        //    //var result = connection.Table<Slide>().OrderByDescending(s => s.LessonId).Take<Slide>(1);
        //    //int lastLessonId = result.First().LessonId;

        //    DownloadSlides(lastOrder);
        //}

        //private void SlideDownloadCompleted()
        //{
        //    SyncCompleted.Invoke();
        //}

        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "EverydayEnglishSQLite.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);

            //if (!System.IO.File.Exists(path))
            //{
            //    File.Create(path);
            //}

            // Create the connection
            var conn = new SQLite.SQLiteConnection(path);
            // Return the database connection
            return conn;
        }

        public Lesson GetNextLesson()
        {
            //throws exception
            //var lessons = connection.Table<Lesson>().Where(l => !l.Completed).OrderBy(l => l.LessonOrder);

            var result = connection.Query<Lesson>("SELECT * FROM Lesson WHERE Completed = 0 ORDER BY LessonOrder LIMIT 1");
            //var result = connection.Table<Lesson>().Where(l => l.Completed == 0).OrderBy(l => l.LessonOrder);
            if (result.Count() == 0)
            {
                return null;
            }
            else
            {
                return result.First();
            }
            

        }


        //public bool SaveCache(Stream data, string fileName)
        //{
        //    try
        //    {

        //        string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //        string localPath = Path.Combine(documentsPath, fileName);
        //        System.IO.File.WriteAllBytes(localPath, bytes); // writes to local storage
        //        byte[] buffer = new byte[data.Length];
        //        data.Read(buffer, 0, buffer.Length);
        //        using (Stream fileStream = new FileStream(localPath, FileMode.Create))
        //        {
        //            fileStream.Write(buffer, 0, buffer.Length);
        //        }
        //        IFolder rootFolder = FileSystem.Current.LocalStorage;
        //        var folder = await rootFolder.CreateFolderAsync("Cache",
        //            CreationCollisionOption.OpenIfExists);
        //        //save cached data
        //        IFile file = await folder.CreateFileAsync(id, CreationCollisionOption.ReplaceExisting);
        //        byte[] buffer = new byte[data.Length];
        //        data.Read(buffer, 0, buffer.Length);
        //        using (Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
        //        {
        //            stream.Write(buffer, 0, buffer.Length);
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        some logging
        //        return false;
        //    }
        //}

        public String GetImagePath(string fileName)
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string localPath = Path.Combine(documentsPath, fileName);
            if (System.IO.File.Exists(localPath))
            {
                return localPath;
                //Stream fileStream = new FileStream(localPath, FileMode.Open);
                
                //return fileStream;
            }

            ////cache folder in local storage
            //IFolder rootFolder = FileSystem.Current.LocalStorage;
            //var folder = await rootFolder.CreateFolderAsync("Cache",
            //    CreationCollisionOption.OpenIfExists);

            //var isExists = await folder.CheckExistsAsync(id);

            //if (isExists == ExistenceCheckResult.FileExists)
            //{
            //    //file exists - load it from cache
            //    IFile file = await folder.GetFileAsync(id);
            //    return await file.OpenAsync(FileAccess.Read);
            //}

            return null;
        }



        //public void DownloadDataFile(string urlString, string fileName)
        //{
        //    var webClient = new WebClient();

        //    webClient.DownloadDataCompleted += (s, e) => {
        //        var bytes = e.Result; // get the downloaded data
        //        string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //        string localPath = Path.Combine(documentsPath, fileName);
        //        System.IO.File.WriteAllBytes(localPath, bytes); // writes to local storage
        //    };

        //    webClient.DownloadDataAsync(new Uri(urlString));

        //}

        public void DownloadImageFile(Media media, Action<string> ImageDownloadComplete)
        {
            var webClient = GetWebClient();

            webClient.DownloadDataCompleted += (s, e) => {
                string error = "";

                try
                {
                    var bytes = e.Result; // get the downloaded data
                    string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    string localPath = Path.Combine(documentsPath, media.ImageFileName);
                    System.IO.File.WriteAllBytes(localPath, bytes); // writes to local storage

                    media.DownloadComplete = true;
                    connection.Update(media);
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    error = ex.Message + ": " + media.ImageFileName;
                }

                ImageDownloadComplete.Invoke(error);
            };

            string urlString = GlobalData.Singleton.DeviceImageUrl + media.ImageFileName;
            webClient.DownloadDataAsync(new Uri(urlString));

        }


        public void DownloadTextFile(string urlString, string fileName)
        {
            var webClient = GetWebClient();

            webClient.DownloadStringCompleted += (s, e) => {
                var text = e.Result; // get the downloaded text
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string localPath = Path.Combine(documentsPath, fileName);
                System.IO.File.WriteAllText(localPath, text); // writes to local storage
            };

            var url = new Uri(urlString);
            webClient.Encoding = Encoding.UTF8;
            webClient.DownloadStringAsync(url);
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

        private void CreateTables()
        {
            connection.CreateTable<Media>();
            connection.CreateTable<Slide>();
            connection.CreateTable<Lesson>();
            connection.CreateTable<SlideMedia>();
            connection.CreateTable<LessonMedia>();
            connection.CreateTable<Category>();
            connection.CreateTable<Accuracy>();

            //connection.DeleteAll<Category>();
            //connection.DeleteAll<Lesson>();
            //connection.DeleteAll<Slide>();
            //connection.DeleteAll<SlideMedia>();
            //connection.DeleteAll<Media>();
            //connection.DeleteAll<LessonMedia>();

            //connection.Query<Slide>("UPDATE Slide SET Completed=0");
            //connection.Query<Lesson>("UPDATE Lesson SET Completed=0");

        }

        public void DownloadLessons(int lastOrder)
        {

            string urlString2 = "http://sukhajata.com/content/lessons.php?lastOrder=" + Convert.ToString(lastOrder);
            WebClient webClient2 = GetWebClient();
            
            webClient2.DownloadStringCompleted += (s, e) =>
            {
                string msg = "";
                try
                {
                    var text = e.Result;
                    Lesson[] lessons = JsonConvert.DeserializeObject<Lesson[]>(text);
                    foreach (Lesson lesson in lessons)
                    {
                        if (connection.Table<Lesson>().Where(l => l.Id == lesson.Id).Count() == 0)
                        {
                            connection.Insert(lesson);
                        }
                    }

                    
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    msg = ex.Message;
                }
                if (msg.Length > 0)
                {
                    ShowError.Invoke(msg);
                }
                else
                {
                    DownloadSlides(lastOrder);
                }
                
            };

            var url2 = new Uri(urlString2);

            webClient2.DownloadStringAsync(url2);

        }

        private void DownloadCategories(Action DownloadCompleted)
        {

            string urlString2 = "http://sukhajata.com/content/categories.php";
            WebClient webClient2 = GetWebClient();

            string msg = "";
            webClient2.DownloadStringCompleted += (s, e) =>
            {
                try
                {
                    var text = e.Result;
                    Category[] categories = JsonConvert.DeserializeObject<Category[]>(text);
                    foreach (Category category in categories)
                    {
                        if (connection.Table<Category>().Where(l => l.Id == category.Id).Count() == 0)
                        {
                            connection.Insert(category);
                        }
                    }

                    
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    msg = ex.Message;
                }

                if (msg.Length > 0)
                {
                    ShowError.Invoke(msg);
                }
                else
                {
                    DownloadCompleted.Invoke();

                }
            };

            var url2 = new Uri(urlString2);

            webClient2.DownloadStringAsync(url2);

        }

        //public void DownloadImage(string fileName)
        //{
        //    //string fileName = connection.Table<Media>().Where(m => m.Id == mediaId).First().ImageFileName;
        //    if (fileName != null && fileName.Length > 0)
        //    {
        //        string imageUrl = GlobalData.ImageUrl + fileName;
        //        DownloadDataFile(imageUrl, fileName);
        //    }
        //}

        public void DownloadLessonMedia(int lastOrder)
        {

            string urlString = "http://sukhajata.com/content/lessonMedia.php?lastLessonOrder=" + Convert.ToString(lastOrder) + "&numLessons=" + Convert.ToString(numLessonsToCache);
            WebClient webClient = GetWebClient();
            webClient.DownloadStringCompleted += (s, e) => {
                string error = "";
                try
                {
                    var text = e.Result; // get the downloaded text
                    Media[] medias = JsonConvert.DeserializeObject<Media[]>(text);
                    foreach (Media media in medias)
                    {
                        if (connection.Table<Media>().Where(m => m.Id == media.Id).Count() == 0)
                        {
                            connection.Insert(media);

                            //connection.Insert(new LessonMedia() { LessonId = lessonId, MediaId = media.Id });
                            //if (media.ImageFileName != null && media.ImageFileName.Length > 0)
                            //{
                            //    string imageUrl = GlobalData.ImageUrl + media.ImageFileName;
                            //    DownloadDataFile(imageUrl, media.ImageFileName);
                            //}
                            //if (media.AudioFileName != null && media.AudioFileName.Length > 0)
                            //{

                            //}
                        }
                    }

                    
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    error = ex.Message;
                }

                if (error.Length > 0)
                {
                    ShowError.Invoke(error);
                }
                else
                {
                    //connection.Query<Slide>("UPDATE Slide SET Completed=1 WHERE LessonId < 3");
                    //connection.Query<Slide>("UPDATE Slide SET Completed=0 WHERE LessonId=3");
                    //connection.Query<Lesson>("UPDATE Lesson SET Completed=1 WHERE Id < 3");
                    //connection.Query<Lesson>("UPDATE Lesson SET Completed=0 WHERE Id=3");
                    SyncCompleted.Invoke();
                }
            };

            var url = new Uri(urlString);
            webClient.Encoding = Encoding.UTF8;
            webClient.DownloadStringAsync(url);


        }

        public void DownloadSlides(int lastLessonOrder)
        {
            string urlString = "http://sukhajata.com/content/slides.php?lastLessonOrder=" + Convert.ToString(lastLessonOrder) + "&numLessons=" + Convert.ToString(numLessonsToCache);
            var url = new Uri(urlString);

            WebClient webClient = GetWebClient();
            webClient.DownloadStringCompleted += (s, e) => {
                string error = "";

                try
                {
                    var text = e.Result; // get the downloaded text
                    SlideWithMedia[] slides = JsonConvert.DeserializeObject<SlideWithMedia[]>(text);

                    
                    foreach (SlideWithMedia slide in slides)
                    {
                        if (connection.Table<Slide>().Where(sl => sl.Id == slide.Id).Count() == 0)
                        {
                            Slide ss = new Slide();
                            ss.Id = slide.Id;
                            ss.LessonId = slide.LessonId;
                            ss.CategoryId = slide.CategoryId;
                            ss.Content = slide.Content;
                            ss.SlideOrder = slide.SlideOrder;
                            connection.Insert(ss);

                            foreach (SlideMedia slideMedia in slide.SlideMedias)
                            {
                                connection.Insert(slideMedia);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    error = ex.Message;
                }

                if (error.Length > 0)
                {
                    ShowError.Invoke(error);
                }
                else
                {
                    DownloadLessonMedia(lastLessonOrder);

                }
            };


            webClient.DownloadStringAsync(url);
        }

        private WebClient GetWebClient()
        {
            WebClient webClient = new WebClient();
            //webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");
            webClient.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            webClient.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            webClient.Headers.Add("Accept-Language", "en-US,en,q=0.8,ro;q=0.6");
            webClient.Encoding = Encoding.UTF8;

            return webClient;
        }

        public IEnumerable<Slide> GetSlides(int lessonId)
        {
            //var slides = connection.Table<Slide>().Where(s => s.LessonId == lessonId && !s.Completed);
            var result = connection.Query<Slide>("SELECT * FROM Slide WHERE LessonId = ? AND Completed = 0", lessonId);
            return result;
        }

        public List<Media> GetSlideMedia(int slideId)
        {
            var slideMedias = connection.Table<SlideMedia>().Where(sm => sm.SlideId == slideId);

            List<Media> medias = new List<Media>();

            foreach(var slideMedia in slideMedias)
            {
                Media media = connection.Table<Media>().Where(m => m.Id == slideMedia.MediaId).First();
                medias.Add(media);
            }

            return medias;
        }

        public int GetTarget(int slideId)
        {
            var result = connection.Table<SlideMedia>().Where(sm => sm.SlideId == slideId && sm.IsTarget == 1);

            if (result.Count() == 0)
            {
                return 0;
            }
            else
            {
                return result.First().MediaId;
            }
        }

        public Media GetMedia(int mediaId)
        {
            var results = connection.Table<Media>().Where(m => m.Id == mediaId);
            if (results.Count() > 0)
            {
                return results.First();
            }

            return null;

        }

        public Queue<Media> GetImageDownloadQueue()
        {
            //get a unique list of images for the next 3 uncompleted lessons
            Queue<Media> mediaQueue = new Queue<Media>();

            var lessons = connection.Table<Lesson>().Where(l => l.Completed == false).OrderBy(l => l.LessonOrder).Take(3);

            foreach (Lesson lesson in lessons)
            {
                var medias = GetLessonMedia(lesson.Id);
                foreach (Media media in medias.Where(m => m.ImageFileName != null && m.ImageFileName.Length > 0 && m.DownloadComplete == false))
                {
                    if (!mediaQueue.Contains(media))
                    {
                        mediaQueue.Enqueue(media);
                    }
                }
            }

            return mediaQueue;
        }



        public List<Media> GetLessonMedia(int lessonId)
        {
            List<Media> lessonMedia = new List<Media>();

            var results = connection.Table<Slide>().Where(s => s.LessonId == lessonId);

            foreach (Slide slide in results)
            {
                var slideMedias = connection.Table<SlideMedia>().Where(m => m.SlideId == slide.Id);
                foreach (SlideMedia slideMedia in slideMedias)
                {
                    Media media = connection.Table<Media>().Where(m => m.Id == slideMedia.MediaId).First();
                    if (!lessonMedia.Contains(media))
                    {
                        lessonMedia.Add(media);

                    }
                }
            }
            //var results = connection.Query<Media>("SELECT m.Id, m.English, m.Thai, m.Phonetic, m.AudioFileName, m.ImageFileName FROM LessonMedia lm INNER JOIN Media m ON m.Id = lm.MediaId WHERE lm.LessonId = ?", lessonId);
            return lessonMedia;
        }

        public void UpdateLesson(Lesson lesson)
        {
            connection.Update(lesson);
        }

        public void UpdateSlide(Slide slide)
        {
            connection.Update(slide);
        }
    }

}