using Microsoft.Maui.Graphics;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using yeQuote;



namespace _yeQuote_
{
    public partial class MainPage : ContentPage
    {
        public void SetQuote(string text)
        {
            NiceAtPingPong.Text = text;

        }

        public void Alert(string txt)
        {
            Extra.IsVisible = true;
            Extra.Text = txt;
        }

        private void SetAlbum(Album album)
        {
            this.BackgroundColor     = Color.FromArgb(album.color);

            NiceAtPingPong.TextColor = Color.FromArgb(album.font);
            YeSignature.TextColor    = Color.FromArgb(album.font);
            Extra.TextColor          = Color.FromArgb(album.font);
        }

        async public Task MainAsync()
        {
            var quote = await Quotes.GetQuote();
            var album = await Albums.GetAlbum();

            SetAlbum(album);
            SetQuote(quote);

        }

        public MainPage()
        {
            if (!App.InternetAccess)
            {
                Alert("Please, Connect to the internet <3");
            }
            
            _ = MainAsync();

            InitializeComponent();
        }        
    }
}
