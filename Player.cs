using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Core;

namespace OsuMusicPlayer
{
    public class Player
    {
        public MediaPlayer MusicPlayer { get { return _musicPlayer; } }
        public Playlist History = new();
        private Common Common = new();
        private Playlist _playlist = null;
        private bool play_end_flag = false;
        private SongsModel SongsModel = new();
        public Playlist Playlist { get { return _playlist; } set { _playlist = value; } }

        private static MediaPlayer _musicPlayer = new()
        {
            Volume = INIT.VOLUME * INIT.VOLUME_RATE,
        };

        public static void Change_volume(int value, bool disable = true)
        {
            if(disable) return;

            _musicPlayer.Volume = value * INIT.VOLUME_RATE;
        }
        static void statusUpdate(object stateInfo)
        {
            Console.WriteLine("test");
        }

        public Player()
        {
            Debug.WriteLine("new Player");
            _musicPlayer.MediaEnded += _musicPlayer_MediaEnded;
            _musicPlayer.MediaFailed += _musicPlayer_MediaFailed;
            Check_info();
        }

        private void _musicPlayer_MediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            Debug.WriteLine($"{args.Error}");
            play_end_flag = false;
            IsStop();
        }

        DispatcherQueue queue = DispatcherQueue.GetForCurrentThread();
        private void _musicPlayer_MediaEnded(MediaPlayer sender, object args)
        {
            Debug.WriteLine("music end");
            play_end_flag = true;
        }

        public async void Check_info()
        {
            Common.PlayerModel.SeekMaxSec = (int)_musicPlayer.PlaybackSession.NaturalDuration.TotalSeconds;
            Common.PlayerModel.SeekSec = (int)_musicPlayer.PlaybackSession.Position.TotalSeconds;

            if (play_end_flag)
            {
                play_end_flag = false;
                IsStop();
            }

            await Task.Delay(100);
            Check_info();
        }

        private void IsStop()
        {
            Common.PlayerModel.IsPlaying = false;
            if (_playlist == null) return;
            Next();
        }

        public void Next()
        {
            var next_metadata = _playlist.Next();
            if (_playlist.Finish) return;
            SongsModel.SelectedItem = next_metadata;
            Play(next_metadata);
        }

        public void Play()
        {
            MusicPlayer.Play();
            Common.PlayerModel.IsPlaying = true;
        }

        public void Play(Metadata metadata)
        {
            if (string.IsNullOrEmpty(App.Settings.OsuPath)) return;

            string path = string.Format("{0}\\Songs\\{1}\\{2}", App.Settings.OsuPath, metadata.MapFolderPath, metadata.AudioFilename);

            Debug.WriteLine(path);

            var uri = new Uri(path);
            MusicPlayer.Source = MediaSource.CreateFromUri(uri);

            History.Add(Common.PlayerModel.Metadata);
            Common.PlayerModel.Metadata = metadata;
            Play();
        }

        public void Play(Playlist playlist)
        {
            if (Common.PlayerModel.Shuffle) playlist.Shuffle();
            _playlist = playlist;
            var metadata = _playlist.Get();
            Play(metadata);
        }

        public void Pause()
        {
            MusicPlayer.Pause();
            Common.PlayerModel.IsPlaying = false;
        }
    }

    public class PlayerModel : INotifyPropertyChanged
    {
        private static Metadata _metadata = new()
        {
            Title = "Test Title",
            Artist = "Test Artist."
        };
        private static int _seekMaxSec = 0;
        private static int _seekSec = 0;
        private static bool _isPlaying = false;
        private static Symbol _isPlaying_icon = Symbol.Play;
        private static int _volume = INIT.VOLUME;
        private static Symbol _volume_icon = Symbol.Volume;
        private static bool _shuffle = true;
        private static Brush _shuffle_ui = new SolidColorBrush(Windows.UI.Color.FromArgb(0xff, 0x00, 0x00, 0xff));
        private static Repeat _repeat = Repeat.None;

        public Metadata Metadata
        {
            get { return _metadata; }
            set
            {
                if (_metadata == value) return;
                _metadata = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Metadata)));
            }
        }

        public int SeekMaxSec
        {
            get { return _seekMaxSec; }
            set
            {
                if (_seekMaxSec == value) return;
                _seekMaxSec = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SeekMaxSec)));
            }
        }

        public int SeekSec
        {
            get { return _seekSec; }
            set
            {
                if (_seekSec == value) return;
                _seekSec = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SeekSec)));
            }
        }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                if (_isPlaying == value) return;
                _isPlaying = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPlaying)));

                IsPlaying_icon = value ? Symbol.Pause : Symbol.Play;
            }
        }

        public Symbol IsPlaying_icon
        {
            get { return _isPlaying_icon; }
            set
            {
                if (_isPlaying_icon == value) return;
                _isPlaying_icon = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPlaying_icon)));
            }
        }


        public int Volume
        {
            get { return _volume; }
            set
            {
                if (_volume == value) return;
                _volume = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Volume)));
                Player.Change_volume(value, false);

                if (value == 0)
                {
                    Volume_icon = Symbol.Mute;
                }
                else
                {
                    Volume_icon = Symbol.Volume;
                }
            }
        }

        public Symbol Volume_icon
        {
            get { return _volume_icon; }
            set
            {
                if (_volume_icon == value) return;
                _volume_icon = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Volume_icon)));
            }
        }

        public bool Shuffle
        {
            get { return _shuffle; }
            set
            {
                if (_shuffle == value) return;
                _shuffle = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Shuffle)));
                Shuffle_ui = value ? new SolidColorBrush(Windows.UI.Color.FromArgb(0xff, 0x00, 0x00, 0xff)) : new SolidColorBrush(Windows.UI.Color.FromArgb(0x00, 0x00, 0x00, 0xff));
            }
        }

        public Brush Shuffle_ui
        {
            get { return _shuffle_ui; }
            set
            {
                if (_shuffle_ui == value) return;
                _shuffle_ui = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Shuffle_ui)));
            }
        }

        public Repeat Repeat
        {
            get { return _repeat; }
            set
            {
                if (_repeat == value) return;
                _repeat = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Repeat)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public enum Repeat
    {
        None,
        Single,
        All
    }
}
