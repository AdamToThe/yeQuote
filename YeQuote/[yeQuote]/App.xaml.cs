namespace _yeQuote_
{
    public partial class App : Application
    {
        public static bool InternetAccess = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        public static Random rng = new Random();

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
