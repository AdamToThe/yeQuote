
using Microsoft.Maui.Controls.Platform;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _yeQuote_
{
    struct Album
    {
        public string name { get; set; } = "The Life Of Pablo";
        public string color { get; set; } = "#F68148";
        public string font  { get; set; } = "#000000";

        public string? image { get; set; } = null;

        public Album() {}
    }

    class Albums
    {
        public async static Task<List<Album>?> AllAlbums()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("albums.json");
            using var reader = new StreamReader(stream);

            string contents = reader.ReadToEnd();

            return JsonSerializer.Deserialize<List<Album>>(contents);
        }

        public async static Task<Album> GetAlbum()
        {
            var albums = await AllAlbums();

            if (albums == null) 
                return new Album();

            return albums[App.rng.Next(albums.Count)];

        }


    }
}
