using System;
using System.Collections.Generic;
using System.Text;

namespace EverydayEnglish3.Content
{
    public class Phrase
    {
        private int ID;
        private string firstLanguage;
        private string secondLanguage;
        private string audioFileName;
        private string imageFileName;
        private bool isAnswer;

        public string FirstLanguage
        {
            get { return firstLanguage; }
        }

        public string SecondLanguage
        {
            get { return secondLanguage; }
        }

        public string AudioFileName
        {
            get { return audioFileName; }
        }

        public string ImageFileName
        {
            get { return imageFileName; }
        }

        public bool IsAnswer
        {
            get { return isAnswer; }
            set
            {
                isAnswer = value;
            }
        }

        public override bool Equals(object obj)
        {
            if (this.ID == ((Phrase)obj).ID)
            {
                return true;
            }

            return false;
        }

    }
}
