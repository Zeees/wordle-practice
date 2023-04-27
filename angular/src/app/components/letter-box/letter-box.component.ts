import { Component, Input } from '@angular/core';
import { WordleLetter } from 'src/app/models/wordle-letter';

@Component({
  selector: 'app-letter-box',
  templateUrl: './letter-box.component.html',
  styleUrls: ['./letter-box.component.scss']
})
export class LetterBoxComponent {

  @Input()
  public letter: WordleLetter = new WordleLetter()

}
