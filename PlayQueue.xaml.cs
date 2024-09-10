using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
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
    public sealed partial class PlayQueue : Page
    {
        public Playlist Playlist { get; set; }
        private Common _common = new();
        public PlayQueue()
        {
            this.InitializeComponent();
            this.Playlist = App.Player.Playlist;
            Queue_Music_ListView.DataContext = Playlist;
            Now_play.DataContext = _common.PlayerModel;
        }

        private void SelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
        {
            //if(sender.SelectedItem == QueueNext)
            //{
            //    Queue_Music_ListView.ItemsSource = Playlist.Songs;
            //}
            //else
            //{
            //    Queue_Music_ListView.ItemsSource = App.Player.History;
            //}
        }
    }
}
