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
using Windows.UI.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OsuMusicPlayer
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private Common _common = new();
        private Search Search = new();
        public MainWindow()
        {
            this.InitializeComponent();
            MusicControls.DataContext = _common.PlayerModel;
        }

        private void Navigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                Nav_Next(typeof(SettingPage));
                return;
            }

            switch (args.SelectedItemContainer?.Tag.ToString())
            {
                case "Music":
                    Nav_Next(typeof(MusicPage));
                    break;
                case "PlayQueue":
                    Nav_Next(typeof(PlayQueue));
                    break;
                default:
                    break;
            }
        }

        private void Nav_Next(Type page)
        {
            if (!Type.Equals(ContentFrame.CurrentSourcePageType, page))
            {
                ContentFrame.Navigate(page);
            }
        }

        private void Navigation_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            Back_Page();
        }

        private void Back_Page()
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }

        private void Navigation_Loaded(object sender, RoutedEventArgs e)
        {
            Navigation.SelectedItem = Navigation.MenuItems[0];  // initialize first page

            ContentFrame.Navigated += ContentFrame_Navigated;   // after navigation setting

        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Navigation.IsBackEnabled = ContentFrame.CanGoBack;
        }

        private void Play_Pause_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_common.PlayerModel.IsPlaying)
            {
                App.Player.Pause();
            }
            else
            {
                App.Player.Play();
            }
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            App.Player.Next();
        }

        private void Mute_Button_Click(object sender, RoutedEventArgs e)
        {
            _common.PlayerModel.Volume = 0;
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (sender.Text == "") return;
            Search.Text = sender.Text;
            if(Type.Equals(ContentFrame.CurrentSourcePageType, typeof(SearchResult)))
            {
                ContentFrame.NavigateToType(typeof(SearchResult), null, new FrameNavigationOptions { IsNavigationStackEnabled = false});
            }
            else
            {
                Nav_Next(typeof(SearchResult));
            }
            Navigation.SelectedItem = null;
        }
    }
}
