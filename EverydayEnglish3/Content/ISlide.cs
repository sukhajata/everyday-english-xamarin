using System;
using System.Collections.Generic;
using System.Text;

namespace EverydayEnglish3.Content
{
    public interface ISlide
    {
        int ID { get; set; }
        int LessonID { get; set; }
        int Type { get; set; }
    }
}
