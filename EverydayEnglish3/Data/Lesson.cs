using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace EverydayEnglish3.Data
{
    public class Lesson
    {
        private bool completed;

        [PrimaryKey]
        public int Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        
        public int LessonOrder { get; set; }

        
        public bool Completed
        {
            get { return completed; }
             set { completed = value; }
        }
    }
}
