using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EverydayEnglish3.Services
{
    public interface ISQLiteHelper
    {
        SQLiteConnection GetConnection();
    }
}
