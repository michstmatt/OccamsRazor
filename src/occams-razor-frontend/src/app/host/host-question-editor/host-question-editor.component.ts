import { Component, OnInit } from '@angular/core';
import { Game } from 'src/app/models/game';
import { HostService } from 'src/app/services/host.service';

@Component({
  selector: 'app-host-question-editor',
  templateUrl: './host-question-editor.component.html',
  styleUrls: ['./host-question-editor.component.css']
})
export class HostQuestionEditorComponent implements OnInit {

  game: Game;
  constructor(private hostService: HostService) {
    this.game = {metadata : {gameId: 1981169330}} as Game;
   }

  ngOnInit(): void {
    this.hostService.setKey('password');
    this.loadGameData();
  }

  async loadGameData(): Promise<void> {
    this.game = await this.hostService.loadQuestions(this.game.metadata.gameId);
  }

  async saveGameData(): Promise<void> {
    await this.hostService.saveQuestions(this.game);
  }

}
