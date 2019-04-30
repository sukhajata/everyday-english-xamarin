using System;
using System.Collections.Generic;
using System.Text;

namespace EverydayEnglish3.Services
{
    public interface IFileHelper
    {
        void WriteToFile(string fileName, byte[] data);
    } 
}
