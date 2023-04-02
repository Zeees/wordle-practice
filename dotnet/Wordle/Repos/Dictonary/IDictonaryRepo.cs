using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Repos.Dictonary
{
    /// <summary>
    ///     Repository for getting dictonary data.
    /// </summary>
    public interface IDictonaryRepo
    {
        /// <summary>
        ///     Check if the word has a valid definiton.
        /// </summary>
        Task<bool> IsWordValid(string word);
        /// <summary>
        ///     Get a random word with the given length.
        /// </summary>
        Task<string> GetRandomWord(int length);
    }
}
