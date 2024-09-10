using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OsuMusicPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchResult : Page
    {
        public Search Search = new();
        public Playlist ResultSongs = new Playlist();
        private SongsModel Songs = new SongsModel();
        public SearchResult()
        {
            this.InitializeComponent();
            Music_ListView.SelectionChanged += Music_ListView_SelectionChanged;
            PageHeader.DataContext = Search;
            Music_ListView.DataContext = ResultSongs;
            foreach (var metasata in Songs.Songs)
            {
                if(SearchMeta(Search.Text, metasata)){
                    ResultSongs.Songs.Add(metasata);
                }
            }
            Debug.WriteLine("Search finish!!!");
        }

        private bool SearchMeta(string target, Metadata metadata)
        {
            if (metadata.Title.ToUpper().Contains(target.ToUpper())) return true;
            if (metadata.Artist.ToUpper().Contains(target.ToUpper())) return true;
            if (metadata.Creator.Contains(target)) return true;
            if (metadata.TitleUnicode.Contains(target)) return true;
            if (metadata.ArtistUnicode.Contains(target)) return true;
            if (metadata.BeatmapID.ToString().Contains(target)) return true;

            return false;
        }
        private void Music_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (App.ListSelect_suppress) return;
            if (Music_ListView.SelectedIndex < 0 && Music_ListView.Items.Count - 1 < Music_ListView.SelectedIndex) return;
            var select = ResultSongs.Songs[Music_ListView.SelectedIndex];
            var playlist = new Playlist(select);
            for (int n = 1; n < Music_ListView.Items.Count; n++)
            {
                var i = Music_ListView.SelectedIndex + n;
                playlist.Add(ResultSongs.Songs[i >= Music_ListView.Items.Count ? i - Music_ListView.Items.Count : n]);
            }
            App.Player.Play(playlist);
        }
    }
}
