import { Component, OnInit } from '@angular/core';
import { PlayerAnswer } from '../../models/player-answer';
import { Question } from 'src/app/models/question';
import { GameMetadata } from '../../models/game-metadata';
import { PlayService } from '../../services/play.service';
@Component({
  selector: 'app-play-answer',
  templateUrl: './play-answer.component.html',
  styleUrls: ['./play-answer.component.css']
})
export class PlayAnswerComponent implements OnInit {

  answer: PlayerAnswer;
  question: Question;
  game: GameMetadata;
  constructor
    (private playService: PlayService) {
    this.game = {gameId: 1981169330} as GameMetadata;
    this.question = {} as Question;
    this.answer = {} as PlayerAnswer;
  }

  ngOnInit(): void {
    this.getCurrentQuestion();
  }

  async submitAnswer(): Promise<void> {
    await this.playService.submitAnswer(this.answer);
  }
  async getCurrentQuestion(): Promise<void> {
    this.question = await this.playService.loadQuestion(this.game.gameId);
  }



}
