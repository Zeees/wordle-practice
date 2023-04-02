using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Wordle.Models.Database;
using Wordle.Models.Domain;
using Wordle.Models.Mapping;
using Wordle.Repos.Dictonary;
using Wordle.Repos.Wordle;
using Wordle.Services;
using Xunit;

namespace WordleTest
{
    public class WordleServiceTest
    {
        [Theory]
        [MemberData(nameof(GameInfoTestData))]
        public async void GetGameInfoTest(Guid gameId, PublicGameInfo? expectedResponse)
        {
            var dictonaryRepo = new Mock<IDictonaryRepo>();
            var wordleRepo = new Mock<IWordleRepo>();
            wordleRepo.Setup(x => x.GetGameInfoAsync(gameId)).ReturnsAsync(
            () =>
            {
                if (gameId != new Guid("7147951d-0f2a-4611-ae8f-28fbdd86c5b6"))
                    return null;
                else
                    return new DbWordleGameInfo()
                    {
                        GameId = new Guid("7147951d-0f2a-4611-ae8f-28fbdd86c5b6"),
                        Attempts = 1,
                        Word = "YODLE",
                        CreatedAt = DateTime.MinValue,
                        LastUpdatedAt = DateTime.MinValue,
                        MaxAttempts = 5,
                        IsDone = false,
                        GuessesJson = "[[{\"letter\":\"L\",\"status\":\"WrongPosition\"},{\"letter\":\"O\",\"status\":\"Correct\"},{\"letter\":\"S\",\"status\":\"WrongLetter\"},{\"letter\":\"E\",\"status\":\"WrongPosition\"},{\"letter\":\"R\",\"status\":\"WrongLetter\"}]]"
                    };
            });
            var mapper = SetupMapper();

            var wordleService = new WordleService(wordleRepo.Object, dictonaryRepo.Object, mapper);

            var actualResponse = await wordleService.GetGameInfoAsync(gameId);
            
            var expectedJson = JsonSerializer.Serialize(expectedResponse);
            var actualJson = JsonSerializer.Serialize(actualResponse);

            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
         public async void CreateGameSessionTest()
        {
            string returnedWord = "YODLE";
            int wordLength = returnedWord.Length;
            int numberOfAttempts = 4;

            var expectedResult = new InitialGameResponse()
            {
                GameID = new Guid("7147951d-0f2a-4611-ae8f-28fbdd86c5b6"),
                NumberOfAttempts = numberOfAttempts,
                WordLength = wordLength
            };

            var dictonaryRepo = new Mock<IDictonaryRepo>();
            dictonaryRepo.Setup(x => x.GetRandomWord(wordLength)).ReturnsAsync(returnedWord);
            var wordleRepo = new Mock<IWordleRepo>();
            wordleRepo.Setup(x => x.CreateGameInfoEntryAsync(returnedWord, numberOfAttempts)).ReturnsAsync(
                new DbWordleGameInfo()
                {
                    GameId = new Guid("7147951d-0f2a-4611-ae8f-28fbdd86c5b6"),
                    Attempts = 0,
                    Word = returnedWord,
                    CreatedAt = DateTime.MinValue,
                    LastUpdatedAt = DateTime.MinValue,
                    MaxAttempts = numberOfAttempts,
                    IsDone = false
                });
            var mapper = SetupMapper();

            var wordleService = new WordleService(wordleRepo.Object, dictonaryRepo.Object, mapper);

            var actualGameSessionResult = await wordleService.InitilizeGameAsync(new InitiateGameRequest()
            {
                NumberOfAttempts = numberOfAttempts,
                WordLength = wordLength
            });

            var expectedJson = JsonSerializer.Serialize(expectedResult);
            var actualJson = JsonSerializer.Serialize(actualGameSessionResult);

            Assert.Equal(expectedJson, actualJson);

        }

        [Theory]
        [MemberData(nameof(GuessTestData))]
        public async void PerformGuessTest(GuessRequest guessRequest, GuessResponse expectedResponse)
        {
            var dictonaryRepo = new Mock<IDictonaryRepo>();
            dictonaryRepo.Setup(x => x.IsWordValid(guessRequest.Guess))
                .ReturnsAsync(
                    true
                );

            var wordleRepo = new Mock<IWordleRepo>();
            wordleRepo.Setup(x => x.GetGameInfoAsync(guessRequest.GameID))
                .ReturnsAsync(
                    () =>
                    {
                        if (guessRequest.GameID != new Guid("7147951d-0f2a-4611-ae8f-28fbdd86c5b6"))
                            return null;
                        else
                            return new DbWordleGameInfo()
                            {
                                GameId = new Guid("7147951d-0f2a-4611-ae8f-28fbdd86c5b6"),
                                Attempts = 0,
                                IsDone = false,
                                Word = "YODLE",
                                MaxAttempts = 5,
                                CreatedAt = DateTime.MinValue,
                                LastUpdatedAt = DateTime.MinValue
                            };
                    });
            var mapper = SetupMapper();

            var wordleService = new WordleService(wordleRepo.Object, dictonaryRepo.Object, mapper);

            var actualResponse = await wordleService.PerformGuessAsync(guessRequest);

            var expectedJson = JsonSerializer.Serialize(expectedResponse);
            var actualJson = JsonSerializer.Serialize(actualResponse);

            Assert.Equal(expectedJson, actualJson);

        }

        private static IMapper SetupMapper()
        {
            var mapperConfig = new MapperConfiguration(x =>
            {
                x.AddProfile(new GameInfoMappingProfile());
            });

            return mapperConfig.CreateMapper();
        }

        public static IEnumerable<object[]> GameInfoTestData =>
            new List<object[]>
            {
                new object[]
                {
                    new Guid("7147951d-0f2a-4611-ae8f-28fbdd86c5b6"),
                    new PublicGameInfo()
                    {
                        GameId = new Guid("7147951d-0f2a-4611-ae8f-28fbdd86c5b6"),
                        Attempts = 1,
                        MaxAttempts = 5,
                        WordLength = 5,
                        Guesses = new List<ICollection<WordleLetter>>()
                        {
                            new List<WordleLetter>()
                            {
                                new WordleLetter()
                                {
                                    Letter = 'L',
                                    Status = LetterStatus.WrongPosition
                                },
                                new WordleLetter()
                                {
                                    Letter = 'O',
                                    Status = LetterStatus.Correct
                                },
                                new WordleLetter()
                                {
                                    Letter = 'S',
                                    Status = LetterStatus.WrongLetter
                                },
                                new WordleLetter()
                                {
                                    Letter = 'E',
                                    Status = LetterStatus.WrongPosition
                                },
                                new WordleLetter()
                                {
                                    Letter = 'R',
                                    Status = LetterStatus.WrongLetter
                                }
                            }
                        }
                    }
                },
                new object[]
                {
                    new Guid("7147951d-0f2a-4611-ae8f-28fbdd86c132"),
                    null
                }
            };

        public static IEnumerable<object[]> GuessTestData =>
            new List<object[]>
            {
                //Normal guess
                new object[] 
                { 
                    new GuessRequest()
                    {
                        GameID = new Guid("7147951d-0f2a-4611-ae8f-28fbdd86c5b6"),
                        Guess = "LOSER"
                    }, 
                    new GuessResponse()
                    {
                        GameID = new Guid("7147951d-0f2a-4611-ae8f-28fbdd86c5b6"),
                        Guess = "LOSER",
                        IsCorrect = false,
                        IsValid = true,
                        Letters = new List<WordleLetter>()
                        {
                            new WordleLetter()
                            {
                                Letter = 'L',
                                Status = LetterStatus.WrongPosition
                            },
                            new WordleLetter()
                            {
                                Letter = 'O',
                                Status = LetterStatus.Correct
                            },
                            new WordleLetter()
                            {
                                Letter = 'S',
                                Status = LetterStatus.WrongLetter
                            },
                            new WordleLetter()
                            {
                                Letter = 'E',
                                Status = LetterStatus.WrongPosition
                            },
                            new WordleLetter()
                            {
                                Letter = 'R',
                                Status = LetterStatus.WrongLetter
                            }
                        }
                    }
                },
                //Guess with lower case letters and the correct word.
                new object[]
                {
                    new GuessRequest()
                    {
                        GameID = new Guid("7147951d-0f2a-4611-ae8f-28fbdd86c5b6"),
                        Guess = "yodle"
                    },
                    new GuessResponse()
                    {
                        GameID = new Guid("7147951d-0f2a-4611-ae8f-28fbdd86c5b6"),
                        Guess = "YODLE",
                        IsCorrect = true,
                        IsValid = true,
                        Letters = new List<WordleLetter>()
                        {
                            new WordleLetter()
                            {
                                Letter = 'Y',
                                Status = LetterStatus.Correct
                            },
                            new WordleLetter()
                            {
                                Letter = 'O',
                                Status = LetterStatus.Correct
                            },
                            new WordleLetter()
                            {
                                Letter = 'D',
                                Status = LetterStatus.Correct
                            },
                            new WordleLetter()
                            {
                                Letter = 'L',
                                Status = LetterStatus.Correct
                            },
                            new WordleLetter()
                            {
                                Letter = 'E',
                                Status = LetterStatus.Correct
                            }
                        }
                    }
                },
                //Guess with invalid game session ID.
                new object[]
                {
                    new GuessRequest()
                    {
                        GameID = new Guid("7147951d-0f2a-4611-ae8f-28fbdd86c5b7"),
                        Guess = "yodle"
                    },
                    null
                }
            };
    }
}