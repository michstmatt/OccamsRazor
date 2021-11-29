import { Component, OnInit } from '@angular/core';
import { PlayerAnswer } from '../../models/player-answer';
@Component({
  selector: 'app-play-answer',
  templateUrl: './play-answer.component.html',
  styleUrls: ['./play-answer.component.css']
})
export class PlayAnswerComponent implements OnInit {

  answer: PlayerAnswer;
  constructor() { 
    this.answer = {} as PlayerAnswer;
  }

  ngOnInit(): void {
  }

  submitAnswer(): void {
    alert(this.answer.answerText);
  }

}
