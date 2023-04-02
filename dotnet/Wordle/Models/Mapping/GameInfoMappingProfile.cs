using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Wordle.Models.Database;
using Wordle.Models.Domain;

namespace Wordle.Models.Mapping
{
    public class GameInfoMappingProfile : Profile
    {
        public GameInfoMappingProfile()
        {
            CreateMap<DbWordleGameInfo, PublicGameInfo>()
                .ForMember(x => x.Guesses, opt => opt.MapFrom(src => GetGuessesFromJson(src.GuessesJson)))
                .ForMember(x => x.WordLength, opt => opt.MapFrom(src => src.Word.Length));
        }

        private ICollection<ICollection<WordleLetter>> GetGuessesFromJson(string json)
        {
            var options = new JsonSerializerOptions
            {
                Converters ={
                    new JsonStringEnumConverter()
                }
            };
            var guesses = JsonSerializer.Deserialize<ICollection<ICollection<WordleLetter>>>(json, options);
            return guesses ?? new List<ICollection<WordleLetter>>();
        }
    }
}
