import { WordleGuess, WordleGuessResponse } from "./wordle-guess-response"

export class WordleGameInfo {
    game_id: string | undefined
    word_length: number = 0
    max_attempts: number = 0
    attempts: number = 0
    guesses: WordleGuess[] = []
}