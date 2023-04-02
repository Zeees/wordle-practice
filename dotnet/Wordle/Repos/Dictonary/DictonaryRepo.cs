using SharedResources.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Models.Dictonary;

namespace Wordle.Repos.Dictonary
{
    public class DictonaryRepo : IDictonaryRepo
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public DictonaryRepo(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetRandomWord(int length)
        {
            using var client = _httpClientFactory.CreateClient("RandomDictonary");

            var results = await HttpClientHelper.GetAsync<ICollection<string>>(client, $"word?length={length}");

            if(results == null || results.Count == 0) { return ""; }

            return results.ElementAt(0);
        }

        public async Task<bool> IsWordValid(string word)
        {
            var client = _httpClientFactory.CreateClient("Dictonary");

            var definition = await HttpClientHelper.GetAsync<ICollection<WordDefinition>>(client, $"entries/en/{word}");

            return definition != null;
        }
    }
}
