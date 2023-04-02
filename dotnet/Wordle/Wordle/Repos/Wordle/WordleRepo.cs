using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Contexts;
using Wordle.Models.Database;

namespace Wordle.Repos.Wordle
{
    public class WordleRepo : IWordleRepo
    {

        private readonly WordleDatabaseContext _wordleContext;

        public WordleRepo(
            WordleDatabaseContext wordleContext)
        {
            _wordleContext = wordleContext;
        }

        public async Task<DbWordleGameInfo> CreateGameInfoEntryAsync(string word, int maxAttempts)
        {
            //Create a new game info object.
            var gameInfo = new DbWordleGameInfo()
            {
                Attempts = 0,
                MaxAttempts = maxAttempts,
                Word = word,
                IsDone = false,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };

            _wordleContext
                .WordleGameInfoEntires
                .Add(gameInfo);

            await _wordleContext.SaveChangesAsync();

            return gameInfo;
        }

        public async Task<bool> DeleteGameInfoEntryAsync(Guid id)
        {
            //Get the game info for the given ID.
            var gameInfo = await _wordleContext
                .WordleGameInfoEntires
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.GameId == id);

            //If there's no game with the given id return false.
            if(gameInfo == null) { return false; }

            //Delete game info entry.
            _wordleContext
                .WordleGameInfoEntires
                .Remove(gameInfo);

            await _wordleContext.SaveChangesAsync();

            return true;
        }

        public async Task<DbWordleGameInfo?> GetGameInfoAsync(Guid id)
        {
            return await _wordleContext
                .WordleGameInfoEntires
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.GameId == id);
        }
    }
}
