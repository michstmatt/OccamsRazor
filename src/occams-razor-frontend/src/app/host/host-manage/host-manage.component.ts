import { Component, OnInit } from '@angular/core';
import { Game } from 'src/app/models/game';
import { GameMetadata } from 'src/app/models/game-metadata';
import { GameState } from 'src/app/models/game-state';
import { Question } from 'src/app/models/question';
import { HostService } from 'src/app/services/host.service';
import { PlayService } from 'src/app/services/play.service';

@Component({
  selector: 'app-host-manage',
  templateUrl: './host-manage.component.html',
  styleUrls: ['./host-manage.component.css']
})
export class HostManageComponent implements OnInit {
  question: Question;
  game: GameMetadata;
  constructor(private hostService: HostService, private playService: PlayService) {
    this.game = {gameId: 1981169330} as GameMetadata;
    this.question = {} as Question;
    this.hostService.setKey("password");
   }

  ngOnInit(): void {
    this.getMetdata();
    this.loadCurrentQuestion();
  }

  async getMetdata(): Promise<void> {
    this.game = await this.playService.getState(this.game.gameId);
  }

  async setCurrentQuestion(): Promise<void> {

  }

  async setState(state:GameState): Promise<void> {
      let tmp: GameMetadata = {gameId: this.game.gameId, state: state} as GameMetadata;
      this.game = await this.hostService.updateState(tmp);
      await this.loadCurrentQuestion();
  }

  async loadCurrentQuestion(): Promise<void>{
    this.question = await this.playService.loadQuestion(this.game.gameId); 
  }

}
