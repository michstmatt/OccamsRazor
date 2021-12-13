import { Component, OnInit } from '@angular/core';
import { Question } from 'src/app/models/question';
import { NotificationService } from 'src/app/services/notification.service';
import { GameMetadata } from '../../models/game-metadata';
import { PlayService } from '../../services/play.service';

@Component({
  selector: 'app-play-manager',
  templateUrl: './play-manager.component.html',
  styleUrls: ['./play-manager.component.css']
})
export class PlayManagerComponent implements OnInit {

  question: Question;
  game: GameMetadata;
  private notificationService: NotificationService
  constructor(
    private playService: PlayService,
  ) {
    this.game = { gameId: 1981169330 } as GameMetadata;
    this.question = {} as Question;
    this.notificationService = new NotificationService(this.game.gameId, playService.getPlayer());
  }

  ngOnInit(): void {

    this.getState();
  }

  async getState(): Promise<void> {
    await this.playService.loadGames();
  }

}
