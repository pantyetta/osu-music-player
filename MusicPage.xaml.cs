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
    public sealed partial class MusicPage : Page
    {
        public SongsModel ViewModel { get; set; }
        private Common Common = new();
        private bool List_loaded = false;
        public MusicPage()
        {
            this.InitializeComponent();
            var SongsModel = new SongsModel();
            this.ViewModel = SongsModel;
            this.DataContext = SongsModel;

            Music_ListView.Loaded += Music_ListView_Loaded;
            Music_ListView.SelectionChanged += Music_ListView_SelectionChanged;
        }

        private void Music_ListView_Loaded(object sender, RoutedEventArgs e)
        {
            List_loaded = true;
        }

        private void Music_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!List_loaded) return;
            if (App.ListSelect_suppress)    return;
            if (Music_ListView.SelectedIndex < 0 && Music_ListView.Items.Count - 1 < Music_ListView.SelectedIndex) return;
            var select = ViewModel.Songs[Music_ListView.SelectedIndex];
            var playlist = new Playlist(select);
            for (int n = 1; n < Music_ListView.Items.Count -1; n++)
            {
                var i = Music_ListView.SelectedIndex + n;
                playlist.Add(ViewModel.Songs[i >= Music_ListView.Items.Count ? i - Music_ListView.Items.Count : n]);
            }
            App.Player.Play(playlist);
        }
               
    }
}
