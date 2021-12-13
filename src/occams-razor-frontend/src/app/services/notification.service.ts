import { Injectable } from '@angular/core';
import { Player } from '../models/player';
import { environment } from 'src/environments/environment';

export class NotificationService {

  host: string
  private webSocket: WebSocket

  constructor(gameId:number, player: Player) {
    this.host = environment.apiUrl;

    const base = `wss://${this.host}/notifications`;
    const url = `${base}/player/${gameId}/${player.name}`;
    this.webSocket = new WebSocket(url);
  }


  getSocket(): WebSocket {
    return this.webSocket;
  }
}
