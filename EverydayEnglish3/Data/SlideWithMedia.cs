using System;
using System.Collections.Generic;
using System.Text;

namespace EverydayEnglish3.Data
{
    public class SlideWithMedia
    {
        public int Id { get; set; }

        public int LessonId { get; set; }

        public int CategoryId { get; set; }

        public string Content { get; set; }

        public int SlideOrder { get; set; }

        public List<SlideMedia> SlideMedias { get; set; }
    }
}
