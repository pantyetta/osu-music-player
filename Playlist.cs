using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuMusicPlayer
{
    public class Playlist
    {
        private bool _finish = false;

        public bool Finish {  get { return _finish; } }
        private ObservableCollection<Metadata> _songs = new();
        public ObservableCollection<Metadata> Songs { get { return _songs; } set { _songs = value; } }
        
        public Metadata Get() {
            if (_songs.Count == 0)
            {
                _finish = true;
                return null;    
            }
            var item = _songs[0];
            _songs.RemoveAt(0);
            return item;
        }

        public void Add(List<Metadata> list)
        {
            foreach (var metadata in list)
            {
                _songs.Add(metadata);
            }
        }
        public void Add(Metadata metadata) {  _songs.Add(metadata); }
        public Playlist(){}
        public Playlist(Metadata metadata) {
            _songs.Add(metadata);
        }
        public Playlist(Metadata[] metadata_list)
        {
            foreach (var item in metadata_list)
            {
                _songs.Add(item);
            }
        }
        public Metadata Next()
        {
            return Get();
        }

        public void Shuffle()
        {
            var n = _songs.Count;
            var rand = new Random();
            for(var i =1; i < n -1; i++)
            {
                var r = rand.Next(i, n-1);
                (_songs[i], _songs[r]) = (_songs[r], _songs[i]);
            }
        }
    }
}
