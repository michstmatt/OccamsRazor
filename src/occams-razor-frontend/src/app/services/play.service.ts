import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Game } from '../models/game';
import { GameMetadata } from '../models/game-metadata';
import { GameState } from '../models/game-state';
import { Player } from '../models/player';
import { PlayerAnswer } from '../models/player-answer';
import { Question } from '../models/question';
import { CommonService } from './common.service';
@Injectable({
  providedIn: 'root'
})
export class PlayService {
  playerNameKey: string = "player-key";
  idKey: string = "game-id";
  host: string;
  session: string = "session";
  constructor(
    private commonService: CommonService
  ) {
    this.host = `${environment.http}://${environment.apiUrl}`;
  }

  authenticatedFetch(url: string, requestOptions: RequestInit): Promise<Response> {
    var headers = new Headers(requestOptions.headers);
    headers.append("Authorization",  `bearer ${this.commonService.getCookie(this.session)}`);
    requestOptions.headers = headers;
    return fetch(url, requestOptions);
  }

  async loadGames(): Promise<GameMetadata[]> {
    const response = await fetch(`${this.host}/api/Play/LoadGames`);
    if (response.ok) {
      return response.json() as Promise<GameMetadata[]>;
    }
    return Promise.reject();
  }

  async loadQuestion(gameId: number): Promise<Question> {
    const requestOptions = {
      method: 'GET',
      headers: { 'Content-Type': 'application/json'},
    };
    const response = await this.authenticatedFetch(`${this.host}/api/Play/GetCurrentQuestion?gameId=${gameId}`, requestOptions);
    if (response.ok) {
      return response.json() as Promise<Question>;
    }
    return Promise.reject();
  }

  async getState(gameId: number): Promise<GameMetadata> {
    const response = await fetch(`${this.host}/api/Play/GetState?gameId=${gameId}`);
    if (response.ok) {
      return response.json() as Promise<GameMetadata>;
    }
    return Promise.reject();
  }

  async submitAnswer(answer: PlayerAnswer) {
    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(answer)
    };
    return await this.authenticatedFetch(`${this.host}/api/Play/submitAnswer`, requestOptions);
  }

  async joinGame(gameId: number, player: Player) : Promise<void> {
    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(player)
    };
    var response = await fetch(`${this.host}/api/authentication/create?gameId=${gameId}`, requestOptions);
    if (!response.ok){
      return Promise.reject();
    }
    var session = await response.text();
    alert(session);
    this.commonService.setCookie(this.session, session.toString());
  }

  getPlayer(): Player {
    return { name: this.commonService.getCookie(this.playerNameKey) } as Player;
  }
}
