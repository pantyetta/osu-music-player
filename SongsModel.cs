using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuMusicPlayer
{
    public class SongsModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public SongsModel() { }

        static private Metadata _selectedItem = null;

        public Metadata SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                }
            }
        }

        static private ObservableCollection<Metadata> _songs = new();

        public ObservableCollection<Metadata> Songs { get { return _songs; } set { _songs = value; } }

        public void AppendSong(Metadata metadata)
        {
            _songs.Add(metadata);
        }

        public void AppendSong(List<Metadata> list)
        {
            foreach (var item in list) {
                AppendSong(item);
            }
        }

        public void AppendSong(IEnumerable<Metadata> list)
        {
            foreach (var item in list)
            {
                AppendSong(item);
            }
        }
    }
}
