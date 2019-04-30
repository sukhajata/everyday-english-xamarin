using System;
using System.Collections.Generic;
using System.Text;

namespace EverydayEnglish3.Services
{
    public interface IAudioService
    {
        bool PlayMP3File(string fileName);
        bool StartRecordAudio();
        bool StopRecordAudio();
        void SpeechToText();
    }
}
