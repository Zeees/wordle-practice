using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wordle.Models.Domain
{
    public class WordleCorrectWordResponse
    {
        [JsonPropertyName("correct_word")]
        public string CorrectWord { get; set; } = string.Empty;
    }
}
