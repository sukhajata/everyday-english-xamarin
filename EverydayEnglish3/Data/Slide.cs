using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace EverydayEnglish3.Data
{
    public class Slide
    {
        private int completed;

        [PrimaryKey]
        public int Id { get; set; }

        
        public int LessonId { get; set; }

        
        public int CategoryId { get; set; }

        [MaxLength(1000)]
        public string Content { get; set; }

        
        public int SlideOrder { get; set; }

        
        public int Completed
        {
            get { return completed; }
            set { completed = value; }
        }
    }
}
