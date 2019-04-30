using EverydayEnglish3.Content;
using EverydayEnglish3.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EverydayEnglish3.Utility
{
    public class JsonHelper
    {
        public void DeserializeMedia(string json, int lessonId)
        {
            Media[] medias = JsonConvert.DeserializeObject<Media[]>(json);
        }


    }
}
