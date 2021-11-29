import { Component, OnInit } from '@angular/core';
import { GameMetadata } from '../../models/game-metadata';
import { HostService } from '../../services/host.service';

@Component({
  selector: 'app-host-create',
  templateUrl: './host-create.component.html',
  styleUrls: ['./host-create.component.css']
})
export class HostCreateComponent implements OnInit {

  game: GameMetadata;
  password: string;
  constructor(private hostService: HostService) {
    this.game = {} as GameMetadata;
    this.password = "";
  }

  ngOnInit(): void {
  }

  async createGame(): Promise<void> {
    await this.hostService.createGame(this.game, this.password);
  }

}
