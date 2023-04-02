using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wordle.Models.Dictonary
{
    public class WordDefinition
    {
        [JsonPropertyName("word")]
        public string Word { get; set; } = string.Empty;
    }
}
