using EverydayEnglish3.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace EverydayEnglish3.Services
{
    public interface IContentManager 
    {
        //void Initialize(Action<string> InitializationComplete);

        //Network
        void SyncData(int numLessonsToCache, Action SyncCompleted, Action<string> ShowError);
        //void DownloadTextFile(string url, string fileName);
        //void DownloadDataFile(string url, string fileName);
        //void DownloadCategories(Action<string> DownloadCompleted);
        void DownloadImageFile(Media media, Action<string> ImageDownloadComplete);
        //void DownloadLessons(Action LessonsDownloadCompleted);
        //void DownloadLessonMedia(int lessonId, Action<string> MediaDownloadCompleted);
        //void DownloadSlides(int lessonId, Action<string> SlideDownloadCompleted);
        

        String GetImagePath(string fileName);
        // void DownloadImage(string fileName);

        //database select
        Queue<Media> GetImageDownloadQueue();
        Lesson GetNextLesson();
        IEnumerable<Lesson> GetLessons();
        int GetTarget(int slideId);
        List<Media> GetSlideMedia(int slideId);
        IEnumerable<Slide> GetSlides(int lessonId);
        List<Media> GetLessonMedia(int lessonId);
        Media GetMedia(int mediaId);
        string GetCategoryInstructions(int categoryId);

        //database update
        void UpdateLesson(Lesson lesson);
        void UpdateSlide(Slide slide);
    }
}
