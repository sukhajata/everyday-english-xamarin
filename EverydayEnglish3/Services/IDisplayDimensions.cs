using System;
using System.Collections.Generic;
using System.Text;

namespace EverydayEnglish3.Services
{
    public interface IDisplayDimensions
    {
        int GetWidth();
        int GetHeight();
        float GetDensity();
        void SetImageSize();
    }
}
