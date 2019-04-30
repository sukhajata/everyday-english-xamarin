using EverydayEnglish3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace EverydayEnglish3.Content
{
    public class MultipleChoiceImageTextSlide : ContentPage
    {
       // private Slide slide;
        private int type;
        private TapGestureRecognizer tapImg;
        private Grid imageGrid;
        private List<Media> mediaList;
        private List<Frame> frames;
        private Dictionary<Image, Frame> images;
        private Media targetMedia;
        private Image targetImage;

        public MultipleChoiceImageTextSlide(List<Media> _mediaList, Media _targetMedia)
        {
            //slide = _slide;
            mediaList = _mediaList;
            targetMedia = _targetMedia;

        }

        public void Setup()
        {
            
            StackLayout layout = new StackLayout();
            layout.BackgroundColor = Color.White;
            frames = new List<Frame>();
            images = new Dictionary<Image, Frame>();

            Label lblTarget = new Label();
            lblTarget.Text = targetMedia.English;
            lblTarget.Style = (Style)App.Current.Resources["labelStyle"];
           // btnInstructions.Text = GlobalData.Singleton.ContentManager.GetCategoryInstructions(slide.CategoryId) + targetMedia.Thai;
            layout.Children.Add(lblTarget);

            imageGrid = new Grid();
            ColumnDefinition colDef = new ColumnDefinition();
            
            imageGrid.ColumnDefinitions.Add(new ColumnDefinition());
            imageGrid.ColumnDefinitions.Add(new ColumnDefinition());
            imageGrid.RowDefinitions.Add(new RowDefinition());
            imageGrid.RowDefinitions.Add(new RowDefinition());

            tapImg = new TapGestureRecognizer();
            tapImg.Tapped += TapImg_Tapped;

            CreateImageFrame(mediaList[0], 0, 0);
            CreateImageFrame(mediaList[1], 0, 1);
            CreateImageFrame(mediaList[2], 1, 0);
            CreateImageFrame(mediaList[3], 1, 1);
            //Frame frame1 = new Frame();
            //frame1.Padding = new Thickness(5);
            //frames.Add(frame1);

            //StackLayout stack1 = new StackLayout();
            //Image img1 = new Image();
            //img1.Source = ImageSource.FromFile(mediaList[0].ImageFileName);
            //img1.GestureRecognizers.Add(tapImg);
            //images.Add(img1, mediaList[0]);
            //stack1.Children.Add(img1);
            //Label lbl1 = new Label();
            //lbl1.Text = mediaList[0].English;
            //stack1.Children.Add(lbl1);

            //frame1.Content = stack1;
            //Grid.SetColumn(frame1, 0);
            //Grid.SetRow(frame1, 0);
            //imageGrid.Children.Add(frame1);

            //Frame frame2 = new Frame();
            //frame2.Padding = new Thickness(5);
            //frames.Add(frame2);

            //StackLayout stack2 = new StackLayout();
            //Image img2 = new Image();
            //img2.Source = ImageSource.FromFile(mediaList[1].ImageFileName);
            //img2.GestureRecognizers.Add(tapImg);
            //images.Add(img2, mediaList[1]);
            //stack2.Children.Add(img2);
            //Label lbl2 = new Label();
            //lbl2.Text = mediaList[1].English;
            //stack2.Children.Add(lbl2);

            //frame2.Content = stack2;
            //Grid.SetColumn(frame2, 1);
            //Grid.SetRow(frame2, 0);
            //imageGrid.Children.Add(frame2);

            //Frame frame3 = new Frame();
            //frame3.Padding = new Thickness(5);
            //frames.Add(frame3);

            //StackLayout stack3 = new StackLayout();
            //Image img3 = new Image();
            //img3.Source = ImageSource.FromFile(mediaList[2].ImageFileName);
            //img3.GestureRecognizers.Add(tapImg);
            //images.Add(img3, mediaList[2]);
            //stack3.Children.Add(img3);
            //Label lbl3 = new Label();
            //lbl3.Text = mediaList[3].English;
            //stack3.Children.Add(lbl3);

            //frame3.Content = stack3;
            //Grid.SetColumn(frame3, 0);
            //Grid.SetRow(frame3, 1);
            //imageGrid.Children.Add(frame3);

            //Frame frame4 = new Frame();
            //frame4.Padding = new Thickness(5);
            //frames.Add(frame4);

            //StackLayout stack4 = new StackLayout();
            //Image img4 = new Image();
            //img4.Source = ImageSource.FromFile(mediaList[3].ImageFileName);
            //img4.GestureRecognizers.Add(tapImg);
            //images.Add(img4, mediaList[3]);
            //stack4.Children.Add(img4);
            //Label lbl4 = new Label();
            //lbl4.Text = mediaList[3].English;
            //stack4.Children.Add(lbl4);

            //frame4.Content = stack4;
            //Grid.SetColumn(frame4, 1);
            //Grid.SetRow(frame4, 1);
            //imageGrid.Children.Add(frame4);

            layout.Children.Add(imageGrid);

            this.Content = layout;
        }

        private void CreateImageFrame(Media media, int col, int row)
        {
            Frame frame1 = new Frame();
            frame1.Style = (Style)App.Current.Resources["frameStyle"];
            frames.Add(frame1);

            StackLayout stack1 = new StackLayout();
            Image img1 = new Image();
            string imgPath = GlobalData.Singleton.ContentManager.GetImagePath(media.ImageFileName);
            img1.Source = ImageSource.FromFile(imgPath);
            // img1.Source = GlobalData.Singleton.DeviceImageUrl + media.ImageFileName;
           // img1.Source = ImageSource.FromStream(() => GlobalData.Singleton.ContentManager.LoadImageFromCache(media.ImageFileName));
            img1.GestureRecognizers.Add(tapImg);

            images.Add(img1, frame1);
            if (media == targetMedia)
            {
                targetImage = img1;
            }

            stack1.Children.Add(img1);
            Label lbl1 = new Label();
            lbl1.Style = (Style)App.Current.Resources["labelStyle"];
            lbl1.Text = media.Thai;
            stack1.Children.Add(lbl1);

            frame1.Content = stack1;
            Grid.SetColumn(frame1, col);
            Grid.SetRow(frame1, row);
            imageGrid.Children.Add(frame1);
        }

        private void TapImg_Tapped(object sender, EventArgs e)
        {
            Image img = (Image)sender;
            Frame frame = images.Where(i => i.Key == img).First().Value;
            //Media selectedMedia = images.Where(i => i.Key == img).First().Value;
            if (img == targetImage)
            {
                frame.BackgroundColor = Color.Green;
                GlobalData.Singleton.AppRoot.OpenNextSlide();

            }
            else
            {
                frame.BackgroundColor = Color.Purple;
            }
            
        }
    }
}
