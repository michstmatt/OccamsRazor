import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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
  constructor(private hostService: HostService, private router:Router) {
    this.game = {} as GameMetadata;
    this.password = "";
  }

  ngOnInit(): void {
  }

  async createGame(): Promise<void> {
    this.game = await this.hostService.createGame(this.game, this.password);
    this.router.navigate(["/host-edit"]);
  }

}
