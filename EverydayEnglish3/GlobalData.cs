using EverydayEnglish3.Data;
using EverydayEnglish3.Services;
using EverydayEnglish3.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EverydayEnglish3
{
    public class GlobalData
    {
        private static GlobalData instance;

        public static GlobalData Singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalData();
                    //instance.LineHeight = Math.Ceiling(instance.FontSize * instance.ContentFontFamily.LineSpacing) + 0.4;
                }
                return instance;
            }
        }

        private GlobalData() { }

        private static string BaseImageUrl = "http://sukhajata.com/images/";

        private static string apiKey = "AIzaSyA4VpkUOz35Z4SB8Rnr1Rp_rpwLuzuDJtY";

        public static string xxhdpiImageUrl = BaseImageUrl + "xxhdpi/";

        public static string xhdpiImageUrl = BaseImageUrl + "xhdpi/";

        public static string hdpiImageUrl = BaseImageUrl + "hdpi/";

        public static string mdpiLargeImageUrl = BaseImageUrl + "mdpi/large/";

        public static string mdpiXLargeImageUrl = BaseImageUrl + "mdpi/xlarge/";

        public static double NormalImageSizeDp = 137.5;

        public static double LargeImageSizeDp = 200;

        public static double XLargeImageSizeDp = 250;

        public IContentManager ContentManager { get; set; }

        public IAudioService AudioService { get; set; }

        public Queue<Slide> SlideQueue { get; set; }

        public App AppRoot { get; set; }

        public string DeviceImageUrl { get; set; }

        public double DeviceImageSizeDp { get; set; }
       
    }
}
