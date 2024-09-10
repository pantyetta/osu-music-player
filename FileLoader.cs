using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using Windows.Storage;
using System.Text.RegularExpressions;

namespace OsuMusicPlayer
{
    public class FileLoader
    {
        public SongsModel SongsModel { get; set; }

        public FileLoader() { 
            this.SongsModel = new SongsModel();
        }

        public async Task<Metadata> LoadAsync(string path, int BeatmapId)
        {
            if(System.IO.Path.GetExtension(path) != ".osu") return null;
            if(!File.Exists(path)) return null;

            Metadata metadata = new();

            string text = await File.ReadAllTextAsync(path);

            //foreach(var block in text.Split("\r\n\r\n"))
            var pattern = @"\r\n\s?\r\n";
            foreach (var block in Regex.Split(text, pattern))
            {
                var line = block.Split("\r\n");
                if (line[0] == "[Difficulty]")
                    break;

                // 先頭行はセクション名のためスキップ
                for (int i = 1; i < line.Length; i++)
                {
                    //string[] content = line[i].Split(":");
                    string[] content = Regex.Split(line[i], "^(.*?):");
                    if (content.Length != 3)
                        Debug.WriteLine("out of array " + string.Join(", ", content));
                    metadata.BeatmapID = BeatmapId;
                    switch (content[1])
                    {
                        case "AudioFilename":
                            metadata.AudioFilename = content[2].TrimStart();
                            break;

                        case "Title":
                            metadata.Title = content[2].TrimStart();
                            break;

                        case "TitleUnicode":
                            metadata.TitleUnicode = content[2].TrimStart();
                            break;

                        case "Artist":
                            metadata.Artist = content[2].TrimStart();
                            break;

                        case "ArtistUnicode":
                            metadata.ArtistUnicode = content[2].TrimStart();
                            break;

                        case "Creator":
                            metadata.Creator = content[2].TrimStart();
                            break;

                        //case "BeatmapID":
                        //    metadata.BeatmapID = int.Parse(content[2]);
                        //    break;

                        default:
                            break;
                    }
                }
            }

            metadata.MapFolderPath = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(path));

            return metadata;
        }

        public async Task LoadAllDifficultAsync(StorageFolder storageFolder)
        {
            var maps = await storageFolder.GetFilesAsync();
            int BeatmapId = -1;
            try {
                BeatmapId = Int32.Parse(storageFolder.DisplayName.Split(" ")[0]);
                } 
            catch(Exception e) {  };
            var list = new List<Metadata>();
            foreach (var map in maps)
            {
                if (map.FileType != ".osu") continue;

                var mapMeta = await LoadAsync(map.Path, BeatmapId);
                if (mapMeta.BeatmapID == -1)
                {
                    Debug.WriteLine("");
                }

                if (list.Count < 1)  list.Add(mapMeta);
                if (list[list.Count - 1].AudioFilename != mapMeta.AudioFilename) list.Add(mapMeta);
            }

            SongsModel.AppendSong(list);
            var db = new Database();
            foreach (var item in list)
            {
                db.add_metadata(item);
            }
        }

        public async void LoadBeatMapsAsync()
        {
            StorageFolder storageFolder = await CheckFoler("OsuFolderToken");
            if (storageFolder == null) return;


            if(App.Settings != null)
            {
                App.Settings.LoadProgressInit = true;
                App.Settings.LoadProgressVisible = Microsoft.UI.Xaml.Visibility.Visible;
            }

            var songsFolder = await storageFolder.GetFolderAsync("Songs");

            var songs = await songsFolder.GetFoldersAsync();
            int count =0;
            if (App.Settings != null)
            {
                App.Settings.LoadProgressMax = songs.Count;
                App.Settings.LoadProgressInit = false;
            }

            foreach (var song in songs)
            {
                await LoadAllDifficultAsync(song);
                count++;
                if (App.Settings != null)
                {
                    App.Settings.LoadProgressValue = count;
                }
            }

            if (App.Settings != null)
            {
                App.Settings.LoadProgressVisible = Microsoft.UI.Xaml.Visibility.Collapsed;
            }

            Debug.WriteLine("[songs folders]" + count);
        }

        private async Task<StorageFolder> CheckFoler(string folderName)
        {
            if (!StorageApplicationPermissions.FutureAccessList.ContainsItem(folderName)) return null;
            return await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folderName);
        }
    }
}
