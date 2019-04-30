using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using EverydayEnglish3.Data;

namespace EverydayEnglish3.Content
{
    public class WritingSlide : ContentPage
    {
        private Media media;
        private Button btnNext;
        private string instructions;

        public WritingSlide(Media _media, string _instructions)
        {
            media = _media;
            instructions = _instructions;
        }

        public void Setup()
        {
            StackLayout layout = new StackLayout();
            layout.BackgroundColor = Color.White;

            layout.Children.Add(new Label() { Style = (Style)App.Current.Resources["instructionsLabelStyle"], Text = instructions });
            layout.Children.Add(new Label() { Style = (Style)App.Current.Resources["highlightLabelStyle"], Text = media.English });
            
            Entry txt = new Entry();
            txt.TextChanged += Txt_TextChanged;
            layout.Children.Add(txt);

            btnNext = new Button() { Style = (Style)App.Current.Resources["buttonStyle"], Text = "ต่อไป", IsEnabled=false};
            btnNext.Clicked += Btn_Clicked;
            layout.Children.Add(btnNext);

            this.Content = layout;
        }

        private void Txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry txt = (Entry)sender;
            if (e.NewTextValue.ToLower() == media.English.ToLower())
            {
                btnNext.IsEnabled = true;
            }
        }

        private void Btn_Clicked(object sender, EventArgs e)
        {
            GlobalData.Singleton.AppRoot.OpenNextSlide();
        }
    }
}
