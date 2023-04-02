import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WordleGameResponse } from '../models/wordle-game-response';
import { WordleGameRequest } from '../models/wordle-game-request';
import { environment } from 'src/environments/environment.prod';
import { Observable } from 'rxjs';
import { WordleGuessResponse } from '../models/wordle-guess-response';
import { WordleGuessRequest } from '../models/wordle-guess-request';

@Injectable({
  providedIn: 'root'
})
export class WordleService {

  private activeGameId: string | undefined = undefined

  constructor(
    private httpClient: HttpClient
  ) { }

  public initWordleGame(wordLength: number, numberOfAttempts: number): Observable<WordleGameResponse> {
    var request = new WordleGameRequest()
    request.word_length = wordLength
    request.number_of_attempts = numberOfAttempts

    return this.httpClient.post<WordleGameResponse>(`${environment.apiUrl}/wordle/start`, request)
  }

  public performGuess(guess: string, gameId: string): Observable<WordleGuessResponse> {
    var request = new WordleGuessRequest()
    request.game_id = gameId
    request.guess = guess

    return this.httpClient.post<WordleGuessResponse>(`${environment.apiUrl}/wordle/guess`, request)
  }

  public abortGame(gameId: string): Observable<any> {
    return this.httpClient.delete<any>(`${environment.apiUrl}/wordle/abort/${gameId}`)
  }
}
