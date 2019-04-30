using EverydayEnglish3.Content;
using EverydayEnglish3.Data;
using EverydayEnglish3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace EverydayEnglish3
{
    public class App : Application
    {
        private Dictionary<Button, Lesson> lessonButtons;
        private Lesson currentLesson;
        private Slide currentSlide;
        private Queue<Media> imageDownloadQueue;
        private bool waiting;
        private bool waitingForData;

        public App()
        {
            // The root page
            
            MainPage = new ContentPage();
            GlobalData.Singleton.AppRoot = this;

            GlobalData.Singleton.ContentManager = DependencyService.Get<IContentManager>();
            GlobalData.Singleton.AudioService = DependencyService.Get<IAudioService>();

            //styles
            Resources = new ResourceDictionary();

            var labelStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property=Label.BackgroundColorProperty, Value=Color.White },
                    new Setter {Property=Label.TextColorProperty, Value=Color.Black },
                    new Setter {Property=Label.FontSizeProperty, Value=22 },
                    new Setter {Property=Label.HorizontalTextAlignmentProperty, Value=TextAlignment.Center }
                }
            };
            Resources.Add("labelStyle", labelStyle);

            var highlightLabelStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property=Label.BackgroundColorProperty, Value=Color.Aqua },
                    new Setter {Property=Label.TextColorProperty, Value=Color.Black },
                    new Setter {Property=Label.FontSizeProperty, Value=24 },
                    new Setter {Property=Label.HorizontalTextAlignmentProperty, Value=TextAlignment.Center }
                }
            };
            Resources.Add("highlightLabelStyle", highlightLabelStyle);

            var instructionsLabelStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property=Label.BackgroundColorProperty, Value=Color.Silver },
                    new Setter {Property=Label.TextColorProperty, Value=Color.Black },
                    new Setter {Property=Label.HorizontalTextAlignmentProperty, Value=TextAlignment.Center },
                    new Setter {Property=Label.FontSizeProperty, Value=20 }
                }
            };
            Resources.Add("instructionsLabelStyle", instructionsLabelStyle);

            var frameStyle = new Style(typeof(Frame))
            {
                Setters =
                {
                    new Setter {Property=Frame.PaddingProperty, Value=new Thickness(3) },
                    new Setter {Property=Frame.BackgroundColorProperty, Value=Color.Silver },
                    
                }
            };
            Resources.Add("frameStyle", frameStyle);

            var frameHighlightStyle = new Style(typeof(Frame))
            {
                Setters =
                {
                    new Setter {Property=Frame.PaddingProperty, Value=new Thickness(3) },
                    new Setter {Property=Frame.BackgroundColorProperty, Value=Color.Green }
                }
            };
            Resources.Add("frameHighlightStyle", frameHighlightStyle);

            var frameWrongStyle = new Style(typeof(Frame))
            {
                Setters =
                {
                    new Setter {Property=Frame.PaddingProperty, Value=new Thickness(3) },
                    new Setter {Property=Frame.BackgroundColorProperty, Value=Color.Purple }
                }
            };
            Resources.Add("frameWrongStyle", frameWrongStyle);

            var buttonStyle = new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter {Property=Button.TextColorProperty, Value=Color.White},
                    new Setter {Property=Button.FontSizeProperty, Value=20 },
                    new Setter {Property=Button.BackgroundColorProperty, Value=Color.Navy }
                }
            };
            Resources.Add("buttonStyle", buttonStyle);

            var gridStyle = new Style(typeof(Grid))
            {
                Setters =
                {
                    new Setter {Property=Grid.PaddingProperty, Value=10 },
                    new Setter {Property=Grid.BackgroundColorProperty, Value=Color.White }
                }
            };
            Resources.Add("gridStyle", gridStyle);
        }

        public void SyncCompleted()
        {
            if (waitingForData)
            {
                currentLesson = GlobalData.Singleton.ContentManager.GetNextLesson();

                //start lesson
                var slides = GlobalData.Singleton.ContentManager.GetSlides(currentLesson.Id);
                PrepareSlides(slides);
            }

            //start image download queue
            imageDownloadQueue = GlobalData.Singleton.ContentManager.GetImageDownloadQueue();
            if (imageDownloadQueue.Count > 0)
            {
                Media media = imageDownloadQueue.Dequeue();
                GlobalData.Singleton.ContentManager.DownloadImageFile(media, ImageDownloadComplete);
            }

           

        }

        public void ShowError(string error)
        {
            MainPage.DisplayAlert("Error syncing data", error, "OK", "Cancel");
        }

        //public void SyncLessonsComplete(string msg)
        //{
        //    if (msg.Length > 0)
        //    {
        //        MainPage.DisplayAlert("Error syncing lessons", msg, "OK", "Cancel");
        //    }
        //    else
        //    {

        //    }
        //}

        //public void LessonsDownloadCompleted(string msg)
        //{
        //    if (msg.Length > 0)
        //    {
        //        MainPage.DisplayAlert("Error downloading lessons", msg, "OK", "Cancel");
        //    }
        //    else
        //    {
        //        currentLesson = GlobalData.Singleton.ContentManager.GetNextLesson();
        //        LoadLesson();

        //    }

        //}

        //public void ShowLessons()
        //{
        //    IEnumerable<Lesson> lessons = GlobalData.Singleton.ContentManager.GetLessons();
        //    lessonButtons = new Dictionary<Button, Lesson>();


        //    StackLayout root = new StackLayout();

        //    foreach (Lesson lesson in lessons)
        //    {
        //        Button btn = new Button();
        //        btn.Text = lesson.Name;
        //        lessonButtons.Add(btn, lesson);
        //        btn.Clicked += BtnLesson_Clicked;
        //        root.Children.Add(btn);
        //    }

        //        ((ContentPage)MainPage).Content = root;
        //}

        //private void LoadLesson()
        //{
        //    var slides = GlobalData.Singleton.ContentManager.GetSlides(currentLesson.Id);

        //    if (slides == null || slides.Count() == 0)
        //    {
        //        GlobalData.Singleton.ContentManager.DownloadLessonMedia(currentLesson.Id, MediaDownloadCompleted);
        //    }
        //    else
        //    {
        //        PrepareSlides(slides);

        //        // OpenNextSlide();
        //    }
        //}



        //private void BtnLesson_Clicked(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;
        //    currentLesson = lessonButtons[btn];

        //    GlobalData.Singleton.ContentManager.DownloadLessonMedia(currentLesson.Id, MediaDownloadCompleted);

        //}

        //public void MediaDownloadCompleted(string msg)
        //{
        //    if (msg.Length > 0)
        //    {
        //        MainPage.DisplayAlert("Error downloading media", msg, "OK", "Cancel");
        //    }
        //    else
        //    {
        //        GlobalData.Singleton.ContentManager.DownloadSlides(currentLesson.Id, SlideDownloadCompleted);
        //    }

        //}

        //public void SlideDownloadCompleted(string msg)
        //{
        //    if (msg.Length > 0)
        //    {
        //        MainPage.DisplayAlert("Error downloading slides", msg, "OK");
        //    }
        //    else
        //    {
        //        //new System.Threading.Thread(new System.Threading.ThreadStart(() => {

        //        //foreach (Media media in lessonMedia)
        //        //{
        //        //    GlobalData.Singleton.ContentManager.DownloadImageFile(media, ImageDownloadComplete);
        //        //}
        //        // })).Start();
        //        var slides = GlobalData.Singleton.ContentManager.GetSlides(currentLesson.Id);
        //        PrepareSlides(slides);

        //        //  OpenNextSlide();


        //    }
        //}

        public void PrepareSlides(IEnumerable<Slide> slides)
        {
            GlobalData.Singleton.SlideQueue = new Queue<Slide>();

            foreach (Slide slide in slides.OrderBy(s => s.SlideOrder))
            {
                GlobalData.Singleton.SlideQueue.Enqueue(slide);
            }

            //are we ready to open the first slide?
            Slide nextSlide = GlobalData.Singleton.SlideQueue.Peek();
            List<Media> medias = GlobalData.Singleton.ContentManager.GetSlideMedia(nextSlide.Id);
            if (medias.Where(m => m.ImageFileName.Length > 0 && !m.DownloadComplete).Count() == 0)
            {
                waiting = false;
                OpenNextSlide();
            }
            else
            {
                MainPage.DisplayAlert("Waiting", "Slides not ready", "OK");
                waiting = true;
            }

          


        }

        public void ImageDownloadComplete(string msg)
        {
            if (!msg.Equals(""))
            {
                MainPage.DisplayAlert("Error downloading image", msg, "OK");
            }
            //else
            //{

                if (imageDownloadQueue.Count > 0)
                {
                    Media media = imageDownloadQueue.Dequeue();
                    GlobalData.Singleton.ContentManager.DownloadImageFile(media, ImageDownloadComplete);
                }

                if (waiting)
                {
                    //can we open the next slide?
                    Slide nextSlide = GlobalData.Singleton.SlideQueue.Peek();
                    List<Media> medias = GlobalData.Singleton.ContentManager.GetSlideMedia(nextSlide.Id);
                    if (medias.Where(m => m.ImageFileName.Length > 0 && !m.DownloadComplete).Count() == 0)
                    {
                        waiting = false;
                        OpenNextSlide();
                    }
                }
                //foreach (SlideMedia slideMed in nextSlide.SlideMedias)
                //{
                //    Media med = GlobalData.Singleton.ContentManager.GetMedia(slideMed.MediaId);

                //    if (!med.DownloadComplete)
                //    {
                //        return;
                //    }
                //}

                // OpenNextSlide();
           // }
        }

        public void OpenNextSlide()
        {
            if (currentSlide != null)
            {
                currentSlide.Completed = 1;
                GlobalData.Singleton.ContentManager.UpdateSlide(currentSlide);
            }

            if (GlobalData.Singleton.SlideQueue.Count == 0)
            {
                currentLesson.Completed = true;
                GlobalData.Singleton.ContentManager.UpdateLesson(currentLesson);
                currentLesson = GlobalData.Singleton.ContentManager.GetNextLesson();

                if (currentLesson == null)
                {
                    MainPage.DisplayAlert("Complete", "No more lessons available", "OK");
                    return;
                }
                else
                {
                    var slides = GlobalData.Singleton.ContentManager.GetSlides(currentLesson.Id);
                    if (slides.Count() == 0)
                    {
                        waitingForData = true;
                        GlobalData.Singleton.ContentManager.SyncData(4, SyncCompleted, ShowError);
                    }
                    else
                    {
                        PrepareSlides(slides);
                    }
                    return;
                }
            }



            currentSlide = GlobalData.Singleton.SlideQueue.Dequeue();

            //Console.WriteLine("opening slide " + currentSlide.Id.ToString() + " type: " + currentSlide.CategoryId.ToString() + " order: " + currentSlide.SlideOrder.ToString());

            List<Media> listMedia = GlobalData.Singleton.ContentManager.GetSlideMedia(currentSlide.Id);
            string instructions = GlobalData.Singleton.ContentManager.GetCategoryInstructions(currentSlide.CategoryId);
            int correctMediaId = GlobalData.Singleton.ContentManager.GetTarget(currentSlide.Id);
            Media targetMedia = null;
            if (listMedia.Count > 0)
            {
                targetMedia = listMedia[0];
            }
            if (correctMediaId > 0)
            {
                targetMedia = listMedia.Where(m => m.Id == correctMediaId).First();
            }

            switch (currentSlide.CategoryId)
            {
                case 1://Multiple Choice Image
                    MultipleChoiceImageSlide mcis = new MultipleChoiceImageSlide(listMedia, targetMedia);
                    mcis.Setup();
                    MainPage = mcis;
                    break;
                case 2:
                    MultipleChoiceTextSlide mcts = new MultipleChoiceTextSlide(listMedia, targetMedia);
                    mcts.Setup();
                    MainPage = mcts;
                    break;
                case 3:
                    MissingWordSlide mws = new MissingWordSlide(listMedia, currentSlide.Content, targetMedia, instructions);
                    mws.Setup();
                    MainPage = mws;
                    break;
                case 4:
                    TeachingSlide ts = new TeachingSlide(listMedia, currentSlide.Content);
                    ts.Setup();
                    MainPage = ts;
                    break;
                case 5:
                    PhoneticsSlide ps = new PhoneticsSlide();
                    break;
                case 6:
                    MatchingPairsSlide mps = new MatchingPairsSlide(listMedia, instructions);
                    mps.Setup();
                    MainPage = mps;
                    break;
                case 8: //Speaking slide
                    //SpeakingSlide ss = new SpeakingSlide(listMedia[0]);
                    //ss.Setup();
                    //MainPage = ss;
                    OpenNextSlide();
                    break;
                case 9:
                    TranslatePhraseSlide tps = new TranslatePhraseSlide();
                    break;
                case 10:
                    MultipleChoiceImageTextSlide mcits = new MultipleChoiceImageTextSlide(listMedia, targetMedia);
                    mcits.Setup();
                    MainPage = mcits;
                    break;
                case 11:
                    MatchingPairsImageSlide mpis = new MatchingPairsImageSlide(listMedia, instructions);
                    mpis.Setup();
                    MainPage = mpis;
                    break;
                case 12: //Matching Pairs Image Text
                    MatchingPairsImageTextSlide mpits = new MatchingPairsImageTextSlide(listMedia, instructions);
                    mpits.Setup();
                    MainPage = mpits;
                    break;
                case 13: //Matching Pairs Writing
                    break;
                case 14:
                    WritingSlide ws = new WritingSlide(listMedia[0], instructions);
                    ws.Setup();
                    MainPage = ws;
                    break;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts            

            var lesson = GlobalData.Singleton.ContentManager.GetNextLesson();

            if (lesson == null)
            {
                waitingForData = true;
            }
            else
            {
                currentLesson = GlobalData.Singleton.ContentManager.GetNextLesson();

                //start lesson
                var slides = GlobalData.Singleton.ContentManager.GetSlides(currentLesson.Id);

                if (slides.Count() == 0)
                {
                    waitingForData = true;
                }
                else
                {
                    PrepareSlides(slides);
                }
            }

            INetworkService networkService = DependencyService.Get<INetworkService>();
            if (networkService.IsConnected())
            {
                //sync 
                GlobalData.Singleton.ContentManager.SyncData(4, SyncCompleted, ShowError);
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }


    }
}
