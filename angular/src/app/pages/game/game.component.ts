import { Component, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs';
import { WordleGameInfo } from 'src/app/models/wordle-game-info';
import { WordleGuess, WordleGuessResponse } from 'src/app/models/wordle-guess-response';
import { WordleLetter } from 'src/app/models/wordle-letter';
import { WordleService } from 'src/app/services/wordle.service';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent implements OnInit {

  @HostListener("document:keydown", ["$event"])
  handleKeyboardEvent(event: KeyboardEvent){
    if(event.key == "Backspace" || event.key == "Delete") {
      this.removeLetter()
    }
    else if(event.key == "Enter") {
      this.submitWord()
    }
    else if(event.key.match(/[a-zA-Z]/i) && event.key.length == 1) {
      this.addLetter(event.key)
    }
  }

  public gameInfo: WordleGameInfo = new WordleGameInfo()
  public currentGuess: string = ""
  public correctWord: string = ""

  public isLoading: boolean = false

  constructor(
    private wordleService: WordleService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ){}

  ngOnInit(): void {

    this.activatedRoute.paramMap.subscribe(x => {
      
      //Check if there's an id param, if it exists, try getting the game info for that id.
      var gameId = x.get("id")

      //If no id exists, go back to the start page.
      if(gameId == undefined) { 
        console.log("Failed to get game info, returning to start.")
        this.router.navigate(['start'])
        return 
      }

      //If it exists, get the game info.
      this.wordleService
      .getGameInfo(gameId)
      .subscribe({
        next: gameInfo => {
          this.gameInfo = gameInfo
          if(gameInfo.attempts == gameInfo.max_attempts) {
            this.getCorrectWord();
          }
        },
        error: e => {
          console.log("Failed to get game info", e)
        }
      })

    })

  }

  private submitWord(): void {
    if(this.currentGuess.length === this.gameInfo.word_length) {

      if(this.gameInfo.game_id == undefined) { return }

      this.wordleService.performGuess(this.currentGuess.toUpperCase(), this.gameInfo.game_id!)
      .pipe(first())
      .subscribe({
        next: resp => {
          
          if(!resp.is_valid) {
            //Invalid guess
            console.log("That's not a word")
          }
          else if(resp.is_correct) {
            //Game done!
            console.log("You won")
            this.addGuess(resp)
          }
          else {
            this.addGuess(resp)
          }
          this.currentGuess = ""

          if(this.gameInfo.guesses.length == this.gameInfo.max_attempts && !resp.is_correct) {
            this.getCorrectWord()
          }

        },
        error: e => {
          console.log("Failed to perform guess", e)
        }
      })

    }
  }

  private getCorrectWord(): void {
    this.wordleService.getCorrectWord(this.gameInfo.game_id ?? "")
    .pipe(first())
    .subscribe({
      next: word => {
        this.correctWord = word.correct_word
      },
      error: e => {
        console.log("Failed to get word")
      }
    })
  }

  private addGuess(resp: WordleGuessResponse): void {
    var guess = new WordleGuess()
    guess.guess = resp.guess ?? ""
    guess.letters = resp.letters
    this.gameInfo.guesses.push(guess)
    this.gameInfo.attempts++
  }

  private addLetter(letter: string): void {
    if(this.currentGuess.length >= this.gameInfo.word_length) { return }
    this.currentGuess = `${this.currentGuess}${letter.toUpperCase()}`
  }

  private removeLetter(): void {
    console.log(this.currentGuess)
    if(this.currentGuess.length > 0) {
      this.currentGuess = this.currentGuess.substring(0, this.currentGuess.length-1)
    }
  }

  //Get all game rows. 
  public getRows(): WordleLetter[][] {

    var rows = []

    for(var i = 0; i < this.gameInfo.max_attempts; i++) {
      rows.push(this.getLetters(i))
    }

    return rows

  }

  //Get the letters for the given index.
  private getLetters(index: number): WordleLetter[] {

    //If a guess exists for the index, return the letters of that guess.
    if(index < this.gameInfo.guesses.length) {
      return this.gameInfo.guesses[index].letters
    }
    //If this is the first index outside the number of guesses, return the current/active word.
    else if(this.gameInfo.guesses.length == index) {
      return this.getCurrentWord()
    }
    //If neither of the above is true, return an array of empty letters.
    else {
      var letters = []
      for(var i = 0; i < this.gameInfo.word_length; i++) {
        letters.push(new WordleLetter())
      }
      return letters
    }

  }

  //Get the current word as an array of letters. 
  private getCurrentWord(): WordleLetter[] {

    var letters = []

    for(var i = 0; i < this.gameInfo.word_length; i++) {

      var letter = new WordleLetter();

      if(this.currentGuess.charAt(i) != undefined) {
        letter.letter = this.currentGuess.charAt(i)
      }

      letters.push(letter)
    }

    return letters

  }

  public getValidLetters(): string[] {
    
    var letters = []
    
    for(var i = 65; i <= 90; i++) {
      letters.push(String.fromCharCode(i))
    }

    return letters
  }

  public isLetterValid(letter: string): boolean {

    var response = true

    this.gameInfo.guesses.forEach(guess => {
      guess.letters.forEach(l => {
        if(l.letter?.toUpperCase() == letter.toUpperCase()) {
          if(l.status == "WrongLetter") {
            response = false
            return
          }
        }
      })

      if(!response) { return }
    })

    return response

  }

  public restart(): void {

    this.correctWord = ""
    this.currentGuess = ""

    this.wordleService.initWordleGame(this.gameInfo.word_length, this.gameInfo.max_attempts)
    .pipe(first())
    .subscribe({
      next: resp => {
        this.router.navigate(["game", resp.game_id])
      },
      error: e => {

      }
    })
  }

  public goBackToStart(): void {
    this.wordleService.setGameId("")
    this.router.navigate(["start"])
  }

  public isDone(): boolean {
    return this.gameInfo.attempts == this.gameInfo.max_attempts
  }

}
