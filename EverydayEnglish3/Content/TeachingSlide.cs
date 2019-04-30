using System;
using System.Collections.Generic;
using System.Text;
using EverydayEnglish3.Data;
using Xamarin.Forms;
using System.Linq;

namespace EverydayEnglish3.Content
{
    public class TeachingSlide : ContentPage
    {
        private List<Media> mediaList;
        private string content;

        private ScrollView scrollView;
        private StackLayout root;
        private Dictionary<Label, Frame> labels;
        private Dictionary<Frame, Media> frames;

        private Media selectedTextMedia;
        private Frame selectedTextFrame;

        private TapGestureRecognizer tapTxt;

        public TeachingSlide(List<Media> _mediaList, string _content)
        {
            mediaList = _mediaList;
            content = _content;

            scrollView = new ScrollView();
            root = new StackLayout();
            labels = new Dictionary<Label, Frame>();
            frames = new Dictionary<Frame, Media>();

            tapTxt = new TapGestureRecognizer();
            tapTxt.Tapped += TapTxt_Tapped;

        }


        public void Setup()
        {
            //content
            root.Children.Add(new Label() { Style = (Style)App.Current.Resources["instructionsLabelStyle"], Text = content });

            foreach (Media media in mediaList)
            {
                CreateTextFrame(media);
            }

            //next button
            Button btnNext = new Button() { Style = (Style)App.Current.Resources["buttonStyle"], Text = "ต่อไป" };
            btnNext.Clicked += BtnNext_Clicked;
            root.Children.Add(btnNext);

            scrollView.Content = root;
            Content = scrollView; 
        }

        private void BtnNext_Clicked(object sender, EventArgs e)
        {
            GlobalData.Singleton.AppRoot.OpenNextSlide();
        }

        public void CreateTextFrame(Media media)
        {
            Frame frame1 = new Frame();
            frame1.Style = (Style)App.Current.Resources["frameStyle"];
            frame1.GestureRecognizers.Add(tapTxt);
            frames.Add(frame1, media);

            StackLayout layout = new StackLayout();

            Label lbl = new Label();
            lbl.Text = media.English;
            lbl.Style = (Style)Application.Current.Resources["labelStyle"];
            lbl.GestureRecognizers.Add(tapTxt);
            labels.Add(lbl, frame1);
            layout.Children.Add(lbl);

            Label lbl2 = new Label() { Style = (Style)App.Current.Resources["labelStyle"], BackgroundColor = Color.Fuchsia, Text = media.Thai };
            lbl2.GestureRecognizers.Add(tapTxt);
            layout.Children.Add(lbl2);
            labels.Add(lbl2, frame1);


            frame1.Content = layout; ;
            root.Children.Add(frame1);
        }

        private void TapTxt_Tapped(object sender, EventArgs e)
        {
            if (selectedTextFrame != null)
            {
                selectedTextFrame.Style = (Style)App.Current.Resources["frameStyle"];
            }

            Label lbl;
            if (sender is Frame)
            {
                selectedTextFrame = sender as Frame;
                lbl = labels.Where(f => f.Value == selectedTextFrame).First().Key;
                //lbl = (Label)frame.Content;

            }
            else
            {
                lbl = sender as Label;
                selectedTextFrame = labels.Where(f => f.Key == lbl).First().Value;
            }
            Media media = frames.Where(l => l.Key == selectedTextFrame).First().Value;

            selectedTextFrame.Style = (Style)App.Current.Resources["frameHighlightStyle"];
            GlobalData.Singleton.AudioService.PlayMP3File(media.AudioFileName);

        }
    }
}
