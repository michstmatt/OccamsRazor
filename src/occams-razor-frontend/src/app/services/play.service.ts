import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Game } from '../models/game';
import { GameMetadata } from '../models/game-metadata';
import { GameState } from '../models/game-state';
import { PlayerAnswer } from '../models/player-answer';
import { Question } from '../models/question';
@Injectable({
  providedIn: 'root'
})
export class PlayService {
  playerNameKey: string;
  idKey: string;
  host: string;
  constructor() {
    this.idKey = "player-id";
    this.playerNameKey = "player-key";
    this.host = environment.apiUrl;
  }

  async loadGames(): Promise<GameMetadata[]> {
    const response = await fetch(`${this.host}/api/Play/LoadGames`);
    if (response.ok) {
      return response.json() as Promise<GameMetadata[]>;
    }
    return Promise.reject();
  }

  async loadQuestion(gameId: string): Promise<Question> {
    const response = await fetch(`${this.host}/api/Play/GetCurrentQuestion?gameId=${gameId}`);
    if (response.ok) {
      return response.json() as Promise<Question>;
    }
    return Promise.reject();
  }

  async getState(gameId: number): Promise<GameState> {
    const response = await fetch(`${this.host}/api/Play/GetState?gameId=${gameId}`);
    if (response.ok) {
      return response.json() as Promise<GameState>;
    }
    return Promise.reject();
  }

  async submitAnswer(answer: PlayerAnswer) {
    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(answer)
    };
    return await fetch(`${this.host}/api/Play/submitAnswer`, requestOptions);
  }
}
