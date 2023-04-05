import { WordleLetter } from "./wordle-letter"

export class WordleGuessResponse {
    game_id: string | undefined
    guess: string | undefined
    is_correct: boolean = false
    is_valid: boolean = false
    guesses: WordleGuess[] = []
}

export class WordleGuess {
    guess: string = ""
    letters: WordleLetter[] = []
}

