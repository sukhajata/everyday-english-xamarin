using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace EverydayEnglish3.Data
{
    public class Category
    {
        [PrimaryKey]
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Instructions { get; set; }
    }
}
