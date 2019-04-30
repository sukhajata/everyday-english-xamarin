using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using EverydayEnglish3.Droid.Services;
using System.Collections.Generic;
using Android.Net;
using Android.Content;
using Java.Lang;
using Android.Speech;
using EverydayEnglish3.Services;

namespace EverydayEnglish3.Droid
{
	[Activity (Label = "EverydayEnglish3", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, IMessageSender
	{
        public static int REQUEST_CODE = 1234;
        private static List<int> screenSizes = new List<int>() { 825, 110, 1650 };

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new EverydayEnglish3.App ());

            SetImageSize();
            
		}

        public void SetImageSize()
        {

            float screenDensity = Resources.DisplayMetrics.Density;

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
                int screenWidth = Resources.DisplayMetrics.WidthPixels;
                int screenHeight = Resources.DisplayMetrics.HeightPixels;

                int screenSize = screenHeight;
                if (screenWidth > screenHeight)
                {
                    screenSize = screenWidth;
                }

                int min = 100000;
                int closest = screenSizes[0];

                foreach (int v in screenSizes)
                {
                    int diff = Java.Lang.Math.Abs(v - screenSize);

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

        public bool IsConnected()
        {
            ConnectivityManager cm = (ConnectivityManager)GetSystemService(Context.ConnectivityService);
            NetworkInfo net = cm.ActiveNetworkInfo; 
            if (net != null && net.IsAvailable && net.IsConnected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == REQUEST_CODE && resultCode == Result.Ok)
            {
                IList<string> list = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                Xamarin.Forms.MessagingCenter.Send<MainActivity, string>(this, "SpeechToText", list[0]);
                
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

    }
}

