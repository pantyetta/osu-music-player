using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuMusicPlayer
{
    public class SettingsModel : INotifyPropertyChanged
    {
        private static string _osuPath = "";
        public string OsuPath
        {
            get => _osuPath;
            set
            {
                if (value == "" || value == _osuPath) return;
                _osuPath = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OsuPath)));
            }
        }

        private static Microsoft.UI.Xaml.Visibility _loadProgressVisible = Microsoft.UI.Xaml.Visibility.Collapsed;
        public Microsoft.UI.Xaml.Visibility LoadProgressVisible
        {
            get => _loadProgressVisible;
            set
            {
                if (value == _loadProgressVisible) return;
                _loadProgressVisible = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoadProgressVisible)));
            }
        }

        private static int _loadProgressValue = 0;
        public int LoadProgressValue
        {
            get => _loadProgressValue;
            set
            {
                if (value == _loadProgressValue) return;
                _loadProgressValue = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoadProgressValue)));
            }
        }

        private static int _loadProgressMax = 0;
        public int LoadProgressMax
        {
            get => _loadProgressMax;
            set
            {
                if (value == _loadProgressMax) return;
                _loadProgressMax = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoadProgressMax)));
            }
        }

        private static bool _loadProgressInit = true;
        public bool LoadProgressInit
        {
            get => _loadProgressInit;
            set
            {
                if (value == _loadProgressInit) return;
                _loadProgressInit = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoadProgressInit)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
