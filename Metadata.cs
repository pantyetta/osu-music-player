using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuMusicPlayer
{
    public class Metadata
    {
        public string AudioFilename { get; set; }
        public string Title { get; set; }
        public string TitleUnicode { get; set; }
        public string Artist { get; set; }
        public string ArtistUnicode { get; set; }
        public string Creator { get; set; }
        public int BeatmapID { get; set; }
        public string MapFolderPath { get; set; }
        public Metadata()
        {
            AudioFilename = "";
            Title = "";
            TitleUnicode = "";
            Artist = "";
            ArtistUnicode = "";
            Creator = "";
            BeatmapID = -1;
            MapFolderPath = "";
        }
    }
}
