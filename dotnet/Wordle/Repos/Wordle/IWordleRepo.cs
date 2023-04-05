using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Models.Database;
using Wordle.Models.Domain;

namespace Wordle.Repos.Wordle
{
    /// <summary>
    ///     Repository for handling Wordle data.
    /// </summary>
    public interface IWordleRepo
    {
        /// <summary>
        ///     Create a new game entry.
        /// </summary>
        /// <param name="word">The correct word.</param>
        /// <param name="maxAttempts">Max number of attempts allowed for this game.</param>
        Task<GameInfo> CreateGameInfoEntryAsync(string word, int maxAttempts);
        /// <summary>
        ///     Get game info for a specific game by it's ID. Null if there's not match.
        /// </summary>
        /// <param name="Id">ID of the game.</param>
        Task<GameInfo?> GetGameInfoAsync(Guid id);
        /// <summary>
        ///     Delete a game entry by it's ID.
        /// </summary>
        Task<bool> DeleteGameInfoEntryAsync(Guid id);
        /// <summary>
        ///     Updates and existing game info entry.
        /// </summary>
        Task<GameInfo> UpdateGameInfoAsync(GameInfo gameInfo);
    }
}
