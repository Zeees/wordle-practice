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
            CreateMap<WordleGameInfo, PublicWordleGameInfo>()
                .ForMember(x => x.WordLength, opt => opt.MapFrom(src => src.Word.Length));

            CreateMap<DbWordleGameInfo, WordleGameInfo>()
                .ForMember(x => x.Guesses, opt => opt.MapFrom(src => GetGuessesFromJson(src.GuessesJson)));

            CreateMap<WordleGameInfo, DbWordleGameInfo>()
                .ForMember(x => x.GuessesJson, opt => opt.MapFrom(src => ToJson<ICollection<WordleGuess>>(src.Guesses)));
        }

        private ICollection<WordleGuess> GetGuessesFromJson(string json)
        {
            var options = new JsonSerializerOptions
            {
                Converters ={
                    new JsonStringEnumConverter()
                }
            };
            var guesses = JsonSerializer.Deserialize<ICollection<WordleGuess>>(json, options);
            return guesses ?? new List<WordleGuess>();
        }

        private string ToJson<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }
    }
}
