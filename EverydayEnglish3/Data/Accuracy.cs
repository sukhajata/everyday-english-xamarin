using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace EverydayEnglish3.Data
{
    public class Accuracy
    {
        [PrimaryKey]
        public int Id { get; set; }

        public int LessonId { get; set; }

        public int SlideId { get;  set; }

        public int MediaId { get; set; }

        public bool Correct { get; set; }
    }
}
