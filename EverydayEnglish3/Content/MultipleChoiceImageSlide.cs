using EverydayEnglish3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace EverydayEnglish3.Content
{
    public class MultipleChoiceImageSlide : ContentPage
    {
        private List<Frame> frames;
        private Dictionary<Image, Media> images;
        private List<Media> mediaList;
        private Media correctMedia;
        private Image correctImage;

        private TapGestureRecognizer tapImg;
        private Grid layout;

        public MultipleChoiceImageSlide(List<Media> _mediaList, Media _targetMedia)
        {
            mediaList = _mediaList;
            correctMedia = _targetMedia;

            layout = new Grid() { Style = (Style)App.Current.Resources["gridStyle"] };

            tapImg = new TapGestureRecognizer();
            tapImg.Tapped += TapImg_Tapped;

            frames = new List<Frame>();
            images = new Dictionary<Image, Media>();
        }

        public void Setup()
        {
            layout.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            layout.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            layout.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            layout.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            layout.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            Label lblTarget = new Label() { Style = (Style)App.Current.Resources["instructionsLabelStyle"], Text = correctMedia.English };
            Grid.SetColumn(lblTarget, 0);
            Grid.SetColumnSpan(lblTarget, 2);
            Grid.SetRow(lblTarget, 0);
            layout.Children.Add(lblTarget);

            
            CreateImageFrame(mediaList[0], 0, 1);
            CreateImageFrame(mediaList[1], 1, 1);
            CreateImageFrame(mediaList[2], 0, 2);
            CreateImageFrame(mediaList[3], 1, 2);

            this.Content = layout;
        }

        private void CreateImageFrame(Media media, int col, int row)
        {
            Frame frame1 = new Frame();
            frame1.Style = (Style)App.Current.Resources["frameStyle"];
            frames.Add(frame1);

            Image img1 = new Image();
            string imgPath = GlobalData.Singleton.ContentManager.GetImagePath(media.ImageFileName);
            img1.Source = ImageSource.FromFile(imgPath);
            //img1.Source = ImageSource.FromStream(() => GlobalData.Singleton.ContentManager.GetImagePath(media.ImageFileName));
            // img1.Source = GlobalData.Singleton.DeviceImageUrl +  media.ImageFileName;
            //img1.Source = ImageSource.FromStream(() => GlobalData.Singleton.ContentManager.LoadCache(media.ImageFileName));
            img1.BackgroundColor = Color.White;
            img1.GestureRecognizers.Add(tapImg);
            images.Add(img1, media);

            if (media == correctMedia)
            {
                correctImage = img1;
            }

            frame1.Content = img1;
            Grid.SetColumn(frame1, col);
            Grid.SetRow(frame1, row);
            layout.Children.Add(frame1);
        }

        private void TapImg_Tapped(object sender, EventArgs e)
        {
            Image img = (Image)sender;
            //Media selectedMedia = images.Where(i => i.Key == img).First().Value;
            if (img == correctImage)
            {
                GlobalData.Singleton.AppRoot.OpenNextSlide();

            }
            else
            {
                Frame frame = frames.Where(f => f.Content == img).First();
                frame.Style = (Style)App.Current.Resources["frameWrongStyle"];
                
            }
        }
    }
}
