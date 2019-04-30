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
using EverydayEnglish3.Droid.Services;
using EverydayEnglish3.Services;

[assembly: Xamarin.Forms.Dependency(typeof(NetworkServiceImplementation))]
namespace EverydayEnglish3.Droid.Services
{
    public class NetworkServiceImplementation : INetworkService
    {
        public bool IsConnected()
        {
            MainActivity main = Xamarin.Forms.Forms.Context as MainActivity;
            return main.IsConnected();
        }
    }
}