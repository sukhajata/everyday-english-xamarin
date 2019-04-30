using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using EverydayEnglish3.Data;
using EverydayEnglish3.Utility;
using System.Linq;

namespace EverydayEnglish3.Content
{
    public class MatchingPairsImageTextSlide : ContentPage
    {
        private List<Media> mediaList;
        private Grid root;

        private Dictionary<Frame, Image> imageFrames;
        private Dictionary<Image, Media> images;
        private Dictionary<Frame, Label> textFrames;
        private Dictionary<Label, Media> labels;

        private Media selectedTextMedia;
        private Media selectedImageMedia;
        private Frame selectedImageFrame;
        private Frame selectedTextFrame;

        //private Grid imgGrid;
        //private Grid txtGrid;

        private TapGestureRecognizer tapImg;
        private TapGestureRecognizer tapTxt;
        private string instructions;

        private int correct = 0;

        public MatchingPairsImageTextSlide(List<Media> _mediaList, string _instructions)
        {
            mediaList = _mediaList;
            instructions = _instructions;

            tapImg = new TapGestureRecognizer();
            tapImg.Tapped += TapImg_Tapped;
            tapTxt = new TapGestureRecognizer();
            tapTxt.Tapped += TapTxt_Tapped;

            root = new Grid();
            root.BackgroundColor = Color.White;
            imageFrames = new Dictionary<Frame, Image>();
            images = new Dictionary<Image, Media>();
            textFrames = new Dictionary<Frame, Label>();
            labels = new Dictionary<Label, Media>();

        }

        public void Setup()
        {
            root.ColumnSpacing = 5;
            root.RowSpacing = 5;

            //first row - instructions
            RowDefinition rowDef0 = new RowDefinition();
            rowDef0.Height = GridLength.Auto;
            root.RowDefinitions.Add(rowDef0);

            Label lbInstructions = new Label();
            lbInstructions.Style = (Style)App.Current.Resources["instructionsLabelStyle"];
            lbInstructions.Text = instructions;
            Grid.SetColumn(lbInstructions, 0);
            Grid.SetColumnSpan(lbInstructions, 2);
            Grid.SetRow(lbInstructions, 0);
            root.Children.Add(lbInstructions);

            //second row - text labels
            RowDefinition rowDef1 = new RowDefinition();
            rowDef1.Height = GridLength.Auto;
            root.RowDefinitions.Add(rowDef1);

            //third row - text labels
            RowDefinition rowDef2 = new RowDefinition();
            rowDef2.Height = GridLength.Auto;
            root.RowDefinitions.Add(rowDef2);

            //fourth row - frames containing image and label
            RowDefinition rowDef3 = new RowDefinition();
            rowDef3.Height = new GridLength(1, GridUnitType.Star);
            root.RowDefinitions.Add(rowDef3);

            //fifth row - frames containing image and label
            RowDefinition rowDef4 = new RowDefinition();
            rowDef4.Height = new GridLength(1, GridUnitType.Star);
            root.RowDefinitions.Add(rowDef4);

            //first column
            ColumnDefinition colDef1 = new ColumnDefinition();
            colDef1.Width = new GridLength(1, GridUnitType.Star);
            root.ColumnDefinitions.Add(colDef1);

            //second column
            ColumnDefinition colDef2 = new ColumnDefinition();
            colDef2.Width = new GridLength(1, GridUnitType.Star);
            root.ColumnDefinitions.Add(colDef2);

            //English labels at top in frames to allow border highlight
            CreateTextFrame(mediaList[0], 0, 1);
            CreateTextFrame(mediaList[1], 1, 1);
            CreateTextFrame(mediaList[2], 0, 2);
            CreateTextFrame(mediaList[3], 1, 2);

            mediaList.Shuffle();

            //Frames containing image and Thai label
            CreateImageFrame(mediaList[0], 0, 3);
            CreateImageFrame(mediaList[1], 1, 3);
            CreateImageFrame(mediaList[2], 0, 4);
            CreateImageFrame(mediaList[3], 1, 4);

            this.Content = root;
        }


        private void CreateTextFrame(Media media, int col, int row)
        {
            Frame frame1 = new Frame();
            frame1.Style = (Style)App.Current.Resources["frameStyle"];
            frame1.GestureRecognizers.Add(tapTxt);
            

            Label lbl = new Label();
            lbl.Text = media.Thai;
            lbl.Style = (Style)Application.Current.Resources["labelStyle"];
            lbl.GestureRecognizers.Add(tapTxt);
            labels.Add(lbl, media);
            textFrames.Add(frame1, lbl);

            frame1.Content = lbl;
            Grid.SetColumn(frame1, col);
            Grid.SetRow(frame1, row);
            root.Children.Add(frame1);
        }

        private void CreateImageFrame(Media media, int col, int row)
        {
            Frame frame1 = new Frame();
            frame1.Style = (Style)App.Current.Resources["frameStyle"];
            frame1.GestureRecognizers.Add(tapImg);

            Grid layout = new Grid();
            RowDefinition rowDef = new RowDefinition();
            rowDef.Height = new GridLength(1, GridUnitType.Star);
            layout.RowDefinitions.Add(rowDef);

            RowDefinition rowDef2 = new RowDefinition();
            rowDef2.Height = GridLength.Auto;
            layout.RowDefinitions.Add(rowDef2);

            frame1.Content = layout;

            Image img1 = new Image();
            string imgPath = GlobalData.Singleton.ContentManager.GetImagePath(media.ImageFileName);
            img1.Source = ImageSource.FromFile(imgPath);
            img1.BackgroundColor = Color.White;
            img1.GestureRecognizers.Add(tapImg);
            images.Add(img1, media);
            imageFrames.Add(frame1, img1);

            Grid.SetRow(img1, 0);
            layout.Children.Add(img1);

            Label lbl = new Label();
            lbl.Style = (Style)Application.Current.Resources["labelStyle"];
            lbl.Text = media.English;
            Grid.SetRow(lbl, 1);
            layout.Children.Add(lbl);

            Grid.SetColumn(frame1, col);
            Grid.SetRow(frame1, row);

            root.Children.Add(frame1);
            
        }

        private void TapImg_Tapped(object sender, EventArgs e)
        {
            if (selectedImageFrame != null)
            {
                //deselect
                selectedImageFrame.Style = (Style)App.Current.Resources["frameStyle"];
            }

            
            Image img;
            if (sender is Frame)
            {
                selectedImageFrame = (Frame)sender;
                img = imageFrames.Where(f => f.Key == selectedImageFrame).First().Value;
            }
            else
            { 
                img = (Image)sender;
                selectedImageFrame = imageFrames.Where(f => f.Value == img).First().Key;
            }

            selectedImageMedia = images.Where(i => i.Key == img).First().Value;

            if (selectedTextMedia == null)
            {
                //no text selected
                //highlight this frame
                selectedImageFrame.Style = (Style)App.Current.Resources["frameHighlightStyle"];

            }
            else
            {
                //text selected already. Is it a match?
                Label selectedLabel = labels.Where(l => l.Value == selectedTextMedia).First().Key;
                Frame txtFrame = textFrames.Where(f => f.Value == selectedLabel).First().Key;
                if (selectedTextMedia == selectedImageMedia)
                {
                    //correct. Remove pair
                    root.Children.Remove(selectedImageFrame);
                    root.Children.Remove(txtFrame);

                    correct++;
                    if (correct == 4)
                    {
                        GlobalData.Singleton.AppRoot.OpenNextSlide();
                    }
                }
                else
                {
                    //wrong. Unselect pair
                    selectedImageFrame.Style = (Style)App.Current.Resources["frameStyle"];

                    txtFrame.Style = (Style)App.Current.Resources["frameStyle"];

                }

                selectedTextFrame = null;
                selectedTextMedia = null;
                selectedImageMedia = null;
                selectedImageFrame = null;

            }

            //other frames
            



        }

        private void TapTxt_Tapped(object sender, EventArgs e)
        {
            //deselect old selection
            if (selectedTextFrame != null)
            {
                selectedTextFrame.Style = (Style)App.Current.Resources["frameStyle"];
            }

            Label lbl;
            
            if (sender is Frame)
            {
                selectedTextFrame = (Frame)sender;
                lbl = (Label)selectedTextFrame.Content;
            }
            else
            {
                lbl = (Label)sender;
                selectedTextFrame = textFrames.Where(f => f.Value == lbl).First().Key;
            }

            selectedTextMedia = labels.Where(l => l.Key == lbl).First().Value;
            

            if (selectedImageMedia == null)
            {
                //no image selected
                selectedTextFrame.Style = (Style)App.Current.Resources["frameHighlightStyle"];

            }
            else
            {
                //image selected already
                //is it a match?
                Image selectedImage = images.Where(i => i.Value == selectedImageMedia).First().Key;
                Frame imageFrame = imageFrames.Where(f => f.Value == selectedImage).First().Key;
                if (selectedImageMedia == selectedTextMedia)
                {
                    //correct.remove pair
                    root.Children.Remove(selectedTextFrame);
                    root.Children.Remove(imageFrame);

                    correct++;

                    if (correct == 4)
                    {
                        GlobalData.Singleton.AppRoot.OpenNextSlide();
                    }
                }
                else
                {
                    //wrong. Unselect pair
                    selectedTextFrame.Style = (Style)App.Current.Resources["frameStyle"];

                    imageFrame.Style = (Style)App.Current.Resources["frameStyle"];


                }

                selectedImageFrame = null;
                selectedTextFrame = null;
                selectedImageMedia = null;
                selectedTextMedia = null;
            }

            //deselect other frames
           


        }



    }
}
