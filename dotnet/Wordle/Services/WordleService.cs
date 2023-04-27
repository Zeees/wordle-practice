using AutoMapper;
using Newtonsoft.Json;
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

        public async Task<PublicWordleGameInfo?> GetGameInfoAsync(Guid gameId)
        {
            var gameInfo = await _wordleRepo.GetGameInfoAsync(gameId);
            if(gameInfo == null) { return null; }
            return _mapper.Map<PublicWordleGameInfo?>(gameInfo);
        }

        public async Task<WordleCorrectWordResponse?> GetCorrectWord(Guid gameId)
        {
            var gameInfo = await _wordleRepo.GetGameInfoAsync(gameId);

            if(gameInfo == null) { return null; }

            //Only return the correct word if the game is marked as done. 
            if(gameInfo.IsDone)
            {
                return new WordleCorrectWordResponse()
                {
                    CorrectWord = gameInfo.Word
                };
            }

            return null;

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
            if(gameInfo.Attempts + 1 > gameInfo.MaxAttempts) { throw new ArgumentException("Maximum guesses exceeded"); }

            //Create a response.
            var response = new GuessResponse()
            {
                GameID = gameInfo.GameId,
                Guess = guess.Guess.ToUpper(),
                IsValid = true
            };

            //Assign statuses to letters.
            response.Letters = GetLetterStatus(guess.Guess, gameInfo.Word);

            //Check if the game is won. 
            if(gameInfo.Word.ToUpper() == guess.Guess.ToUpper()) { response.IsCorrect = true; }

            //Update game info.
            gameInfo.Attempts++;
            gameInfo.Guesses.Add(
                new WordleGuess()
                {
                    Guess = guess.Guess,
                    Letters = response.Letters
                });

            if(gameInfo.Attempts >= gameInfo.MaxAttempts)
            {
                gameInfo.IsDone = true;
            }

            await _wordleRepo.UpdateGameInfoAsync(gameInfo);

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

                if (answer[i] == guess[i]) 
                { 
                    letter.Status = LetterStatus.Correct;
                }
                else if (answer.Contains(guess[i])) 
                { 
                    //Get the number of occurances in both the guess and answer.
                    var indexesAnswer = GetIndexesOf(guess[i], answer);
                    var indexesGuess = GetIndexesOf(guess[i], guess);

                    //If the number of occurances are the same or if the answer has more, the letter is in the wrong position.
                    if(indexesAnswer.Count() >= indexesGuess.Count())
                    {
                        letter.Status = LetterStatus.WrongPosition;
                    }
                    //If there are more occurances in the guess than the answer, then check to see if the player has already
                    //guessed the correct amount of the given letter. If this is the case, mark the letter as wrong.
                    else if(indexesAnswer.Count() < indexesGuess.Count())
                    {
                        var correctCount = 0;

                        foreach(var index in indexesAnswer)
                        {
                            if (answer[index] == guess[index]) { correctCount++; }
                        }

                        if(correctCount <= indexesGuess.Count())
                        {
                            letter.Status = LetterStatus.WrongLetter;
                        }

                    }
                    
                }

                response.Add(letter);
            }

            return response;
        
        }

        private IEnumerable<int> GetIndexesOf(char c, string s)
        {
            var indexes = new List<int>();

            for(int i = s.IndexOf(c); i > -1; i = s.IndexOf(c, i+1))
            {
                indexes.Add(i);
            }

            return indexes;
        }

        
    }
}
