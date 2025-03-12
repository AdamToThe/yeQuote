using _yeQuote_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace yeQuote
{
    internal class Quotes
    {
        private static readonly string[] QuotesSet = {
            "i'm nice at ping pong",
            "Everybody want to know what I’d do if I didn’t win. I guess we’ll never know",
            "I don't need your pussy bitch; I'm on my own dick.",
            "I hate when I'm on a plane and I wake up with a water bottle next to me like oh great now I gotta be responsible for this water bottle",
            "Everyone makes mistakes, I just make them in public",
            "George Bush doesn't care about black people."
        };


        public static async Task<List<string>?> LoadQuotes()
        {
            using (HttpClient httpc = new HttpClient())
            {
                return await httpc.GetFromJsonAsync<List<string>>("https://raw.githubusercontent.com/AdamToThe/yeQuote/main/quotes.json");
            }
        }

        public static async Task<string> GetQuote()
        {
            List<string> quotes = QuotesSet.ToList();

            if (App.InternetAccess)
                quotes = await LoadQuotes() ?? quotes;
               

            return quotes[App.rng.Next(quotes.Count)];
        }
    }
}
