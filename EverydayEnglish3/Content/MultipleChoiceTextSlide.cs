using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using EverydayEnglish3.Data;
using System.Linq;

namespace EverydayEnglish3.Content
{
    public class MultipleChoiceTextSlide : ContentPage
    {

        private List<Media> mediaList;
        private Dictionary<Label, Media> labels;
        private Dictionary<Media, Frame> frames;
        private Media targetMedia;
        private TapGestureRecognizer tapLbl;
        private StackLayout layout;
      

        public MultipleChoiceTextSlide(List<Media> _mediaList, Media _targetMedia)
        {
            mediaList = _mediaList;

            targetMedia = _targetMedia;

            labels = new Dictionary<Label, Media>();
            frames = new Dictionary<Media, Frame>();
            layout = new StackLayout();

            tapLbl = new TapGestureRecognizer();
            tapLbl.Tapped += TapLbl_Tapped; ;
        }

        public void Setup()
        {
            layout.BackgroundColor = Color.White;

            layout.Children.Add(new Label() { Style = (Style)App.Current.Resources["highlightLabelStyle"], Text = targetMedia.English });
            
            foreach (Media media in mediaList)
            {
                CreateTextLabel(media);
            }
            
            this.Content = layout;
        }

        private void CreateTextLabel(Media media)
        {
            Frame frame1 = new Frame();
            frame1.Style = (Style)App.Current.Resources["frameStyle"];
            frame1.GestureRecognizers.Add(tapLbl);
            frames.Add(media, frame1);

            Label lbl1 = new Label();
            lbl1.Style = (Style)App.Current.Resources["labelStyle"];
            lbl1.Text = media.Thai;
            lbl1.GestureRecognizers.Add(tapLbl);
            labels.Add(lbl1, media);

            frame1.Content = lbl1;
            layout.Children.Add(frame1);
        }

        private void TapLbl_Tapped(object sender, EventArgs e)
        {
            Frame frame;
            Label lbl;
            Media media;

            if (sender is Label)
            {
                lbl = (Label)sender;
                media = labels.Where(l => l.Key == lbl).First().Value;
                frame = frames.Where(f => f.Key == media).First().Value;
            }
            else
            {
                frame = sender as Frame;
                lbl = (Label)frame.Content;
                media = media = labels.Where(l => l.Key == lbl).First().Value;
            }

            if (media == targetMedia)
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
