import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { first } from 'rxjs';
import { WordleService } from 'src/app/services/wordle.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-start',
  templateUrl: './start.component.html',
  styleUrls: ['./start.component.scss']
})
export class StartComponent {

  public initGameForm = this.formBuilder.group({
    number_of_attempts: [5, [Validators.required, Validators.max(15), Validators.min(1)]],
    word_length: [5, [Validators.required, Validators.max(8), Validators.min(2)]]
  })

  public isLoading: boolean = false

  constructor(
    private formBuilder: FormBuilder,
    private wordleService: WordleService,
    private router: Router
  ) {
    console.log(environment)
  }

  initGame(): void {

    if(!this.initGameForm.valid) { return }

    this.isLoading = true

    this.wordleService
      .initWordleGame(this.initGameForm.controls.word_length.value ?? 5, this.initGameForm.controls.number_of_attempts.value ?? 5)
      .pipe(first())
      .subscribe({
        next: x => {

          this.isLoading = false
          this.wordleService.setGameId(x.game_id ?? "")
          this.router.navigate(["game", x.game_id])

        },
        error: e => {
          console.log("Failed to initilize game", e)
        }
      })

  }

}
