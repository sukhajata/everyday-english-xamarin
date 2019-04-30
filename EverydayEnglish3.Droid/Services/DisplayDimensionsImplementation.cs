using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EverydayEnglish3.Services;
using Android.Util;
using Android.Content.PM;
using Android.Content.Res;

[assembly: Xamarin.Forms.Dependency(typeof(EverydayEnglish3.Droid.Services.DisplayDimensionsImplementation))]
namespace EverydayEnglish3.Droid.Services
{
    public class DisplayDimensionsImplementation : IDisplayDimensions
    {
        private DisplayMetrics metrics;
        private static List<int> screenSizes = new List<int>() { 825, 110, 1650 };

        public DisplayDimensionsImplementation()
        {
            metrics = new DisplayMetrics();
            
        }

        public float GetDensity()
        {
            return metrics.Density;
        }

        public int GetHeight()
        {
            return metrics.HeightPixels;
            //var dp = (int)(heightPixels / metrics.Density);
            //return dp;

        }

        public int GetWidth()
        {
            return metrics.WidthPixels;
           // int dp = (int)(widthPixels / metrics.Density);
            //return dp;
        }

        public void SetImageSize()
        {
            
            float screenDensity = metrics.Density;

            //screens above mdpi are always normal size
            GlobalData.Singleton.DeviceImageSizeDp = GlobalData.NormalImageSizeDp;

            if (screenDensity > 2.5)
            {
                //xxhdpi
                GlobalData.Singleton.DeviceImageUrl = GlobalData.xxhdpiImageUrl;

            }
            else if (screenDensity > 1.5)
            {
                //xhdpi
                GlobalData.Singleton.DeviceImageUrl = GlobalData.xhdpiImageUrl;
            }
            else if (screenDensity > 1)
            {
                //hdpi
                GlobalData.Singleton.DeviceImageUrl = GlobalData.hdpiImageUrl;
            }
            else
            {
                //mdpi
                int screenWidth = metrics.WidthPixels;
                int screenHeight = metrics.HeightPixels;

                int screenSize = screenHeight;
                if (screenWidth > screenHeight)
                {
                    screenSize = screenWidth;
                }

                int min = 100000;
                int closest = screenSizes[0];

                foreach (int v in screenSizes)
                {
                    int diff = Math.Abs(v - screenSize);

                    if (diff < min)
                    {
                        min = diff;
                        closest = v;
                    }
                }

                if (screenSize > 960)
                {
                    //xlarge
                    GlobalData.Singleton.DeviceImageUrl = GlobalData.mdpiXLargeImageUrl;
                    GlobalData.Singleton.DeviceImageSizeDp = GlobalData.XLargeImageSizeDp;
                }
                else
                {
                    //large
                    GlobalData.Singleton.DeviceImageUrl = GlobalData.mdpiLargeImageUrl;
                    GlobalData.Singleton.DeviceImageSizeDp = GlobalData.LargeImageSizeDp;
                }
            }

        }
    }
}