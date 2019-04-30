using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using EverydayEnglish3.Data;
using EverydayEnglish3.Services;

namespace EverydayEnglish3.Content
{
    public class SpeakingSlide : ContentPage
    {
        private Media media;
        private StackLayout layout;
        private Label lblText;
        private Image mic;
        private TapGestureRecognizer tapTxt;
        private TapGestureRecognizer tapMic;
        private IAudioService audioService;
        private Label lblMessage;
        //private bool recording;

        public SpeakingSlide(Media _media)
        {
            media = _media;

            layout = new StackLayout();
            layout.BackgroundColor = Color.White;
            mic = new Image();
            lblText = new Label();
            tapTxt = new TapGestureRecognizer();
            tapTxt.Tapped += TapTxt_Tapped;
            tapMic = new TapGestureRecognizer();
            tapMic.Tapped += TapMic_Tapped;
            audioService = DependencyService.Get<IAudioService>();
            lblMessage = new Label();
        }

        public void Setup()
        {
            lblText.Text = media.English;
            lblText.GestureRecognizers.Add(tapTxt);
            lblText.Style = (Style)App.Current.Resources["labelStyle"];
            layout.Children.Add(lblText);

            mic.Source = ImageSource.FromFile("mic.png");
            mic.GestureRecognizers.Add(tapMic);
            layout.Children.Add(mic);

            lblMessage.Style = (Style)App.Current.Resources["labelStyle"];
            layout.Children.Add(lblMessage);

            //subscribe to receive result of speech to text
            MessagingCenter.Subscribe<IMessageSender, string>(this, "SpeechToText", (s, arg) =>
            {
                string result = arg as string;
                if (result.Equals(media.English))
                {
                    GlobalData.Singleton.AppRoot.OpenNextSlide();
                }
                else
                {
                    lblMessage.Text = "ลองอีกครั้ง";
                }
            });

            this.Content = layout;
        }

        private void TapTxt_Tapped(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void TapMic_Tapped(object sender, EventArgs e)
        {
            //if (!recording)
            //{

            //    recording = true;
            //    layout.BackgroundColor = Color.Green;
            //   // audioService.StartRecordAudio();
            //}
            //else
            //{

            //    audioService.StopRecordAudio();
            //}
            audioService.SpeechToText();
        }


    }
}
