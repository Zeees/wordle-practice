using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Models.Domain;

namespace Wordle.Services
{
    /// <summary>
    ///     Service to start and play a game of Wordle.
    /// </summary>
    public interface IWordleService
    {
        /// <summary>
        ///     Get game info for a given game ID.
        /// </summary>
        Task<PublicWordleGameInfo?> GetGameInfoAsync(Guid gameId);
        /// <summary>
        ///     Get the correct word for the given game ID. This will only return a value if the game exists and is marked as done. 
        /// </summary>
        /// <returns></returns>
        Task<WordleCorrectWordResponse?> GetCorrectWord(Guid gameId);
        /// <summary>
        ///     Initilize the game.
        /// </summary>
        Task<InitialGameResponse> InitilizeGameAsync(InitiateGameRequest request);
        /// <summary>
        ///     Abort an active game.
        /// </summary>
        /// <param name="gameId">Id of the game to abort.</param>
        Task<bool> AbortGameAsync(Guid gameId);
        /// <summary>
        ///     Make a guess for an active game. If no active game is found null will be returned.
        /// </summary>
        Task<GuessResponse?> PerformGuessAsync(GuessRequest guess);
    }
}
