using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;


namespace OsuMusicPlayer
{
    public class Database
    {
        QueryFactory db;


        public Database()
        {
            
            var _connection = new SqliteConnection("DataSource=./data.sqlite");
            _connection.Open();

            var query = _connection.CreateCommand();
            query.CommandText = "CREATE TABLE IF NOT EXISTS Songs (id INTEGER primary key autoincrement, AudioFilename text, Title text, TitleUnicode text, Artist text, ArtistUnicode text, Creator text, BeatmapID int, MapFolderPath text)";
            query.ExecuteNonQuery();

            var compiler = new SqliteCompiler();
            db = new QueryFactory(_connection, compiler);

            db.Logger = compiled =>
            {
                Console.WriteLine("SqLite");
                Console.WriteLine(compiled.ToString());
                Console.WriteLine("Parameters: " + string.Join(", ", compiled.Bindings));
            };
        }

        public int add_metadata(Metadata metadata)
        {
            return db.Query("Songs").Insert(new
            {
                AudioFilename = metadata.AudioFilename,
                Title = metadata.Title,
                TitleUnicode = metadata.TitleUnicode,
                Artist = metadata.Artist,
                ArtistUnicode = metadata.ArtistUnicode,
                Creator = metadata.Creator,
                BeatmapID = metadata.BeatmapID,
                MapFolderPath = metadata.MapFolderPath,
            });
        }

        public IEnumerable<Metadata> get_Songs()
        {
            return db.Query("Songs").Get<Metadata>();
        }

        public void reset_metadata()
        {
            db.Query("Songs").Delete();
        }
    }
}