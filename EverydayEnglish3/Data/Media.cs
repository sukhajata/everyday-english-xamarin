using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace EverydayEnglish3.Data
{
    public class Media
    {

        [PrimaryKey]
        public int Id { get; set; }

        public string Thai { get; set; }

        public string English { get; set; }

        public string Phonetic { get; set; }

        [MaxLength(100)]
        public string AudioFileName { get; set; }
        
        [MaxLength(100)]
        public string ImageFileName { get; set; }
        
        public bool DownloadComplete { get; set; }

       

    }
}
