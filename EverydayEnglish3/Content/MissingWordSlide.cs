using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using EverydayEnglish3.Data;
using System.Linq;

namespace EverydayEnglish3.Content
{
    public class MissingWordSlide : ContentPage
    {
        private List<Media> mediaList;
        private string word;
        private Media targetMedia;
        private string instructions;

        private StackLayout root;
        private Dictionary<Frame, Label> textFrames;
        private Dictionary<Label, Media> labels;

        private Media selectedTextMedia;
        private Frame selectedTextFrame;

        private TapGestureRecognizer tapTxt;

        public MissingWordSlide(List<Media> _mediaList, string _content, Media _targetMedia, string _instructions)
        {
            mediaList = _mediaList;
            word = _content;
            instructions = _instructions;
            targetMedia = _targetMedia;

            root = new StackLayout();
            textFrames = new Dictionary<Frame, Label>();
            labels = new Dictionary<Label, Media>();

            tapTxt = new TapGestureRecognizer();
            tapTxt.Tapped += TapTxt_Tapped;

        }


        public void Setup()
        {
            //instructions
            root.Children.Add(new Label() { Style = (Style)App.Current.Resources["instructionsLabelStyle"], Text = instructions });

            //sentence
            string sentence = targetMedia.English.Replace(word, "__________");
            root.Children.Add(new Label() { Style = (Style)App.Current.Resources["highlightLabelStyle"], Text = sentence });
            root.Children.Add(new Label() { Style = (Style)App.Current.Resources["highlightLabelStyle"], Text = targetMedia.Thai });

            CreateTextFrame(mediaList[0]);
            CreateTextFrame(mediaList[1]);
            CreateTextFrame(mediaList[2]);
            CreateTextFrame(mediaList[3]);

            Content = root;
        }

        public void CreateTextFrame(Media media)
        {
            Frame frame1 = new Frame();
            frame1.Style = (Style)App.Current.Resources["frameStyle"];
            frame1.GestureRecognizers.Add(tapTxt);


            Label lbl = new Label();
            lbl.Text = media.English;
            lbl.Style = (Style)Application.Current.Resources["labelStyle"];
            lbl.GestureRecognizers.Add(tapTxt);
            labels.Add(lbl, media);
            textFrames.Add(frame1, lbl);

            frame1.Content = lbl;
            root.Children.Add(frame1);
        }

        private void TapTxt_Tapped(object sender, EventArgs e)
        {
            Label lbl;
            Frame frame;
            if (sender is Frame)
            {
                frame = sender as Frame;
                lbl = (Label)frame.Content;

            }
            else
            {
                lbl = sender as Label;
                frame = textFrames.Where(f => f.Value == lbl).First().Key;
            }
            Media media = labels.Where(l => l.Key == lbl).First().Value;
            GlobalData.Singleton.AudioService.PlayMP3File(media.AudioFileName);

            if (media.English.ToLower().Equals(word.ToLower()))
            {
                frame.Style = (Style)App.Current.Resources["frameHighlightStyle"];
                GlobalData.Singleton.AppRoot.OpenNextSlide();
            }
            else
            {
                frame.Style = (Style)App.Current.Resources["frameWrongStyle"];
            }

        }
    }
}
