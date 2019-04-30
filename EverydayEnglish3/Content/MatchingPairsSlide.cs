using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using EverydayEnglish3.Data;
using EverydayEnglish3.Utility;
using System.Linq;

namespace EverydayEnglish3.Content
{
    public class MatchingPairsSlide : ContentPage
    {
        private List<Media> mediaList;
        private Grid root;

        private Dictionary<Frame, Label> englishFrames;
        private Dictionary<Label, Media> englishLabels;
        private Dictionary<Frame, Label> thaiFrames;
        private Dictionary<Label, Media> thaiLabels;

        private Media selectedEnglishMedia;
        private Media selectedThaiMedia;

        //private Grid imgGrid;
        //private Grid txtGrid;

        private TapGestureRecognizer tapEnglish;
        private TapGestureRecognizer tapThai;
        private string instructions;
        private int correct;


        public MatchingPairsSlide (List<Media> _mediaList, string _instructions)
        {
            mediaList = _mediaList;
            instructions = _instructions;

            root = new Grid();
            englishFrames = new Dictionary<Frame, Label>();
            englishLabels = new Dictionary<Label, Media>();
            thaiFrames = new Dictionary<Frame, Label>();
            thaiLabels = new Dictionary<Label, Media>();

            tapEnglish = new TapGestureRecognizer();
            tapEnglish.Tapped += TapEnglish_Tapped;
            tapThai = new TapGestureRecognizer();
            tapThai.Tapped += TapThai_Tapped;
        }

        public void Setup()
        {
            //first row - instructions
            root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            //first column
            ColumnDefinition colDef1 = new ColumnDefinition();
            colDef1.Width = new GridLength(1, GridUnitType.Star);
            root.ColumnDefinitions.Add(colDef1);

            //second column
            ColumnDefinition colDef2 = new ColumnDefinition();
            colDef2.Width = new GridLength(1, GridUnitType.Star);
            root.ColumnDefinitions.Add(colDef2);

            Label lblInstructions = new Label() { Text = instructions, Style = (Style)App.Current.Resources["highlightLabelStyle"] };
            Grid.SetRow(lblInstructions, 0);
            Grid.SetColumn(lblInstructions, 0);
            Grid.SetColumnSpan(lblInstructions, 2);
            root.Children.Add(lblInstructions);

            CreateEnglishFrame(mediaList[0], 0, 1);
            CreateEnglishFrame(mediaList[1], 1, 1);
            CreateEnglishFrame(mediaList[2], 0, 2);
            CreateEnglishFrame(mediaList[3], 1, 2);

            mediaList.Shuffle();

            CreateThaiFrame(mediaList[0], 0, 3);
            CreateThaiFrame(mediaList[1], 1, 3);
            CreateThaiFrame(mediaList[2], 0, 4);
            CreateThaiFrame(mediaList[3], 1, 4);

            Content = root;
        }

        private void CreateEnglishFrame(Media media, int col, int row)
        {
            Frame frame1 = new Frame();
            frame1.Style = (Style)App.Current.Resources["frameStyle"];
            frame1.GestureRecognizers.Add(tapEnglish);


            Label lbl = new Label();
            lbl.Text = media.English;
            lbl.Style = (Style)Application.Current.Resources["labelStyle"];
            lbl.GestureRecognizers.Add(tapEnglish);
            englishLabels.Add(lbl, media);
            englishFrames.Add(frame1, lbl);

            frame1.Content = lbl;
            Grid.SetColumn(frame1, col);
            Grid.SetRow(frame1, row);
            root.Children.Add(frame1);
        }

        private void CreateThaiFrame(Media media, int col, int row)
        {
            Frame frame1 = new Frame();
            frame1.Style = (Style)App.Current.Resources["frameStyle"];
            frame1.GestureRecognizers.Add(tapThai);


            Label lbl = new Label();
            lbl.Text = media.Thai;
            lbl.Style = (Style)Application.Current.Resources["labelStyle"];
            lbl.GestureRecognizers.Add(tapThai);
            thaiLabels.Add(lbl, media);
            thaiFrames.Add(frame1, lbl);

            frame1.Content = lbl;
            Grid.SetColumn(frame1, col);
            Grid.SetRow(frame1, row);
            root.Children.Add(frame1);
        }

        private void TapThai_Tapped(object sender, EventArgs e)
        {
            Label lbl;
            Frame selectedFrame;
            if (sender is Frame)
            {
                selectedFrame = (Frame)sender;
                lbl = (Label)selectedFrame.Content;
            }
            else
            {
                lbl = (Label)sender;
                selectedFrame = thaiFrames.Where(f => f.Value == lbl).First().Key;
            }

            selectedThaiMedia = thaiLabels.Where(l => l.Key == lbl).First().Value;

            if (selectedEnglishMedia == null)
            {
                selectedFrame.Style = (Style)App.Current.Resources["frameHighlightStyle"];
            }
            else
            {
                Label englishLabel = englishLabels.Where(l => l.Value == selectedEnglishMedia).First().Key;
                Frame englishFrame = englishFrames.Where(f => f.Value == englishLabel).First().Key;

                if (selectedEnglishMedia == selectedThaiMedia)
                {
                    //match. remove
                    correct++;

                    if (correct == 4)
                    {
                        GlobalData.Singleton.AppRoot.OpenNextSlide();
                    }

                    root.Children.Remove(selectedFrame);
                    root.Children.Remove(englishFrame);

                    
                }
                else
                {
                    //not a match. deselect
                    selectedFrame.Style = (Style)App.Current.Resources["frameStyle"];
                    englishFrame.Style = (Style)App.Current.Resources["frameStyle"];
                }

                selectedEnglishMedia = null;
                selectedThaiMedia = null;
            }
        }

        private void TapEnglish_Tapped(object sender, EventArgs e)
        {
            Label lbl;
            Frame selectedFrame;
            if (sender is Frame)
            {
                selectedFrame = (Frame)sender;
                lbl = (Label)selectedFrame.Content;
            }
            else
            {
                lbl = (Label)sender;
                selectedFrame = englishFrames.Where(f => f.Value == lbl).First().Key;
            }

            selectedEnglishMedia = englishLabels.Where(l => l.Key == lbl).First().Value;

            if (selectedThaiMedia == null)
            {
                selectedFrame.Style = (Style)App.Current.Resources["frameHighlightStyle"];
            }
            else
            {
                Label thaiLabel = thaiLabels.Where(l => l.Value == selectedThaiMedia).First().Key;
                Frame thaiFrame = thaiFrames.Where(f => f.Value == thaiLabel).First().Key;

                if (selectedEnglishMedia == selectedThaiMedia)
                {
                    //match. remove
                    correct++;

                    if (correct == 4)
                    {
                        GlobalData.Singleton.AppRoot.OpenNextSlide();
                    }

                    root.Children.Remove(selectedFrame);
                    root.Children.Remove(thaiFrame);
                }
                else
                {
                    //not a match. deselect
                    selectedFrame.Style = (Style)App.Current.Resources["frameStyle"];
                    thaiFrame.Style = (Style)App.Current.Resources["frameStyle"];
                }

                selectedEnglishMedia = null;
                selectedThaiMedia = null;
            }
        }
    }
}
