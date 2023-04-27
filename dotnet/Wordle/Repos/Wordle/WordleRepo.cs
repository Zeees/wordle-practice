using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Contexts;
using Wordle.Models.Database;
using Wordle.Models.Domain;

namespace Wordle.Repos.Wordle
{
    public class WordleRepo : IWordleRepo
    {

        private readonly WordleDatabaseContext _wordleContext;
        private readonly IMapper _mapper;

        public WordleRepo(
            WordleDatabaseContext wordleContext,
            IMapper mapper)
        {
            _wordleContext = wordleContext;
            _mapper = mapper;
        }

        public async Task<WordleGameInfo> CreateGameInfoEntryAsync(string word, int maxAttempts)
        {
            //Create a new game info object.
            var gameInfo = new DbWordleGameInfo()
            {
                Attempts = 0,
                MaxAttempts = maxAttempts,
                Word = word,
                IsDone = false,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                GuessesJson = "[]"
            };

            _wordleContext
                .WordleGameInfoEntires
                .Add(gameInfo);

            await _wordleContext.SaveChangesAsync();

            return _mapper.Map<WordleGameInfo>(gameInfo);
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

        public async Task<WordleGameInfo?> GetGameInfoAsync(Guid id)
        {
            var info = await _wordleContext
                .WordleGameInfoEntires
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.GameId == id);

            return _mapper.Map<WordleGameInfo>(info);
        }

        public async Task<WordleGameInfo> UpdateGameInfoAsync(WordleGameInfo gameInfo)
        {
            var entry = _mapper.Map<DbWordleGameInfo>(gameInfo);

            _wordleContext
                .WordleGameInfoEntires
                .Update(entry);

            await _wordleContext .SaveChangesAsync();

            return gameInfo;
        }
    }
}
