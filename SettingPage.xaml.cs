using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.System;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OsuMusicPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingPage : Page
    {

        public string Version
        {
            get
            {
                var version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
                return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
            }
        }

        public SettingPage()
        {
            this.InitializeComponent();
            this.DataContext = App.Settings;
        }

        private async void Choose_Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new Window();
            var hwnd = WindowNative.GetWindowHandle(window);

            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.FileTypeFilter.Add("*");

            InitializeWithWindow.Initialize(folderPicker, hwnd);
            
            var folder = await folderPicker.PickSingleFolderAsync();
            if (folder == null) return;
            try
            {
                if(await folder.TryGetItemAsync("osu!.exe") != null)
                {
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("OsuFolderToken", folder);
                    App.Settings.OsuPath = folder.Path;
                }
            }catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            FileLoader fileLoader = new FileLoader();
            var SongsModel = new SongsModel();
            SongsModel.AppendSong(await fileLoader.LoadAsync(@"D:\osu!\Songs\40440 S3RL - Pika Girl\S3RL - Pika Girl (Takuya) [Easy].osu", -1));
        }


        private void Load_AllMap_ClickAsync(object sender, RoutedEventArgs e)
        {
            FileLoader fileLoader = new FileLoader();
            Database database = new Database();
            database.reset_metadata();
            fileLoader.LoadBeatMapsAsync();
        }

        private async void RepositoryCard_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/pantyetta/osu-music-player"));
        }
    }
}
