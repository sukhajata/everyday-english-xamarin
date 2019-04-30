using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using EverydayEnglish3.Services;
using EverydayEnglish3.Droid.Services;
using Java.Util;

using System.IO;


using Android.Net;
using Android.Speech;

[assembly:Xamarin.Forms.Dependency(typeof(AudioServiceImplementation))]
namespace EverydayEnglish3.Droid.Services
{
    public class AudioServiceImplementation : IAudioService
    {
        private SoundPool soundPool;
        private Dictionary<int, int> soundIds;
        private AudioManager audioManager;
        private MediaRecorder mediaRecorder;
        private MediaPlayer mediaPlayer;
        private string outputPath;
        private float actVolume;
        private float maxVolume;
        private float volume;
        

        public AudioServiceImplementation()
        {
            soundPool = new SoundPool(1, Android.Media.Stream.Music, 0);
            audioManager = (AudioManager)Application.Context.GetSystemService(Context.AudioService);
            mediaPlayer = new MediaPlayer();
            mediaRecorder = new MediaRecorder();    
            actVolume = (float)audioManager.GetStreamVolume(Android.Media.Stream.Music);
            maxVolume = audioManager.GetStreamMaxVolume(Android.Media.Stream.Music);
            volume = actVolume / maxVolume;

            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            outputPath = Path.Combine(documentsPath, "speaking.3gpp");
            
        }

        public bool StartRecordAudio()
        {
            //mediaRecorder.SetAudioSource(AudioSource.Mic);
            //mediaRecorder.SetOutputFormat(OutputFormat.ThreeGpp);
            //mediaRecorder.SetAudioEncoder(AudioEncoder.AmrNb);
            //mediaRecorder.SetOutputFile(outputPath);
            //mediaRecorder.Prepare();
            //mediaRecorder.Start();

            return true;
        }

        public bool HasMicrophone()
        {
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;

            if (rec != "android.hardware.microphone")
            {
                return false;
            }

            return true;
        }

        public void SpeechToText()
        {
            MainActivity main = Xamarin.Forms.Forms.Context as MainActivity;

            if (main.IsConnected())
            {
                var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
                //voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, Application.Context.GetString(Resource.String.messageSpeakNow));
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);

                try
                {
                    main.StartActivityForResult(voiceIntent, MainActivity.REQUEST_CODE);
                }
                catch (Exception ex)
                {

                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    string error = ex.Message;
                    string stackTrace = ex.StackTrace;
                }
            }
        }

        public bool StopRecordAudio()
        {
            //mediaRecorder.Stop();
            //mediaRecorder.Reset();

            //SpeechRecognitionActivity speechActivity = new SpeechRecognitionActivity();
            MainActivity main = Xamarin.Forms.Forms.Context as MainActivity;

            if (main.IsConnected())
            {
                var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
                //voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, Application.Context.GetString(Resource.String.messageSpeakNow));
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);

                try
                {
                    main.StartActivityForResult(voiceIntent, MainActivity.REQUEST_CODE);
                }
                catch (Exception ex)
                {
                    
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    string error = ex.Message;
                    string stackTrace = ex.StackTrace;
                }
            }
            else
            {
                return false;
            }
        
            //var speech = SpeechClient.Create();
            //var response = speech.SyncRecognize(new RecognitionConfig()  
            //{
            //    Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
            //    SampleRate = 16000,
            //}, RecognitionAudio.FromFile("audio.raw"));
            //foreach (var result in response.Results)
            //{
            //    foreach (var alternative in result.Alternatives)
            //    {
            //        Console.WriteLine(alternative.Transcript);
            //    }
            //}

            return true;
        }

       

        public bool PlayMP3File(string fileName)
        {
            return true;   
        }

        public bool PlayPhrase(int phraseId)
        {
            if (soundIds.ContainsKey(phraseId))
            {
                soundPool.Play(soundIds[phraseId], volume, volume, 0, 0, 1);
                return true;
            }

            return false;
        }

        public bool LoadAudioFiles(Dictionary<int, string> sounds)
        {
            soundIds = new Dictionary<int, int>();

            foreach (KeyValuePair<int, string> pair in sounds)
            {
                int soundId = soundPool.Load(Application.Context.Assets.OpenFd(pair.Value), 1);
                soundIds.Add(pair.Key, soundId);
            }

            return true;
        }


    }
}