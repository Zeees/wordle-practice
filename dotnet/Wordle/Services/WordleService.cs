using AutoMapper;
using Wordle.Models.Domain;
using Wordle.Repos.Dictonary;
using Wordle.Repos.Wordle;

namespace Wordle.Services
{
    public class WordleService : IWordleService
    {

        private readonly IWordleRepo _wordleRepo;
        private readonly IDictonaryRepo _dictonaryRepo;
        private readonly IMapper _mapper;

        public WordleService(
            IWordleRepo wordleRepo,
            IDictonaryRepo dictonaryRepo,
            IMapper mapper)
        {   
            _wordleRepo = wordleRepo;
            _dictonaryRepo = dictonaryRepo; 
            _mapper = mapper;
        }

        public async Task<bool> AbortGameAsync(Guid gameId)
        {
            return await _wordleRepo.DeleteGameInfoEntryAsync(gameId);
        }

        public async Task<PublicGameInfo?> GetGameInfoAsync(Guid gameId)
        {
            var gameInfo = await _wordleRepo.GetGameInfoAsync(gameId);
            if(gameInfo == null) { return null; }
            return _mapper.Map<PublicGameInfo?>(gameInfo);
        }

        public async Task<InitialGameResponse> InitilizeGameAsync(InitiateGameRequest request)
        {
            //Get a randomized word.
            var word = await _dictonaryRepo.GetRandomWord(request.WordLength);
            //Create a game session entry.
            var gameInfo = await _wordleRepo.CreateGameInfoEntryAsync(word.ToUpper(), request.NumberOfAttempts);

            return new InitialGameResponse()
            {
                GameID = gameInfo.GameId,
                WordLength = word.Length,
                NumberOfAttempts = gameInfo.MaxAttempts
            };
        }

        public async Task<GuessResponse?> PerformGuessAsync(GuessRequest guess)
        {
            var isWordValid = await _dictonaryRepo.IsWordValid(guess.Guess);

            if(!isWordValid)
            {
                return new GuessResponse()
                {
                    GameID = guess.GameID,
                    IsCorrect = false,
                    IsValid = false,
                    Guess = guess.Guess,
                    Letters = new List<WordleLetter>()
                };
            }

            //Get game info.
            var gameInfo = await _wordleRepo.GetGameInfoAsync(guess.GameID);

            //Check if the game info exists and that the provided guess has the correct length.
            if(gameInfo == null) { return null; }
            if(gameInfo.Word.Length != guess.Guess.Length) { throw new ArgumentException("The provided guess needs to match the game settings word length."); }

            //Create a response.
            var response = new GuessResponse()
            {
                GameID = gameInfo.GameId,
                Guess = guess.Guess.ToUpper(),
                IsValid = true
            };

            response.Letters = GetLetterStatus(guess.Guess, gameInfo.Word);

            if(gameInfo.Word.ToUpper() == guess.Guess.ToUpper()) { response.IsCorrect = true; }

            return response;
        }

        private ICollection<WordleLetter> GetLetterStatus(string guess, string answer) 
        {
            guess = guess.ToUpper();
            answer = answer.ToUpper();
            
            var response = new List<WordleLetter>();

            for(var i = 0; i < answer.Length; i++) 
            {
                var letter = new WordleLetter()
                {
                    Letter = guess[i],
                    Status = LetterStatus.WrongLetter
                };

                if (answer[i] == guess[i]) { letter.Status = LetterStatus.Correct; }
                else if (answer.Contains(guess[i])) { letter.Status = LetterStatus.WrongPosition; }

                response.Add(letter);
            }

            return response;
        
        }
    }
}
