import { Component, OnInit } from '@angular/core';
import { GameMetadata } from '../../models/game-metadata';
import { PlayService } from '../../services/play.service';

@Component({
  selector: 'app-play-manager',
  templateUrl: './play-manager.component.html',
  styleUrls: ['./play-manager.component.css']
})
export class PlayManagerComponent implements OnInit {

  game: GameMetadata;
  constructor
    (private playService: PlayService) {
    this.game = {} as GameMetadata;
  }

  ngOnInit(): void {

    this.getState();
  }

  async getState(): Promise<void> {
    await this.playService.loadGames();
  }

}
