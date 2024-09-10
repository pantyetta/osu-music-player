using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuMusicPlayer
{
    public class Search : INotifyPropertyChanged
    {
        public static string _text { get; set; }
        public string Text
        {
            get { return _text; }
            set
            {
                if (value == "" || value == _text) return;
                _text = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
