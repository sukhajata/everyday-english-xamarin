using System;
using EverydayEnglish3.iOS.Services;
using EverydayEnglish3.Services;
using AVFoundation;
using Foundation;

[assembly:Xamarin.Forms.Dependency(typeof(AudioServiceImplementation))]
namespace EverydayEnglish3.iOS.Services
{
    public class AudioServiceImplementation : IAudioService
    {
        #region Private Variables
        private AVAudioPlayer backgroundMusic;
        private AVAudioPlayer soundEffect;
        private string backgroundSong = "";
        #endregion

        #region Computed Properties
        public float BackgroundMusicVolume
        {
            get { return backgroundMusic.Volume; }
            set { backgroundMusic.Volume = value; }
        }

        public bool MusicOn { get; set; } = true;
        public float MusicVolume { get; set; } = 0.5f;

        public bool EffectsOn { get; set; } = true;
        public float EffectsVolume { get; set; } = 1.0f;
        #endregion

        public AudioServiceImplementation()
        {
            // Initialize
            ActivateAudioSession();
        }

        public void ActivateAudioSession()
        {
            // Initialize Audio
            var session = AVAudioSession.SharedInstance();
            session.SetCategory(AVAudioSessionCategory.Ambient);
            session.SetActive(true);
        }

        public bool PlayMP3File(string fileName)
        {
            NSUrl songURL;

            // Music enabled?
            if (!EffectsOn) return false;

            // Any existing sound effect?
            if (soundEffect != null)
            {
                //Stop and dispose of any sound effect
                soundEffect.Stop();
                soundEffect.Dispose();
            }

            // Initialize background music
            songURL = new NSUrl("Sounds/" + fileName);
            NSError err;
            soundEffect = new AVAudioPlayer(songURL, "mp3", out err);
            soundEffect.Volume = EffectsVolume;
            soundEffect.FinishedPlaying += delegate {
                soundEffect = null;
            };
            soundEffect.NumberOfLoops = 0;
            soundEffect.Play();

            return true;
        }
    }
}