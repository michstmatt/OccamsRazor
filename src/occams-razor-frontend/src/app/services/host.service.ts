import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { GameMetadata } from '../models/game-metadata';
import { CommonService } from './common.service';
import { Question } from '../models/question';
import { Game } from '../models/game';

@Injectable({
  providedIn: 'root'
})
export class HostService {

  host: string;
  private cookieName: string = "host-key";
  constructor(private commonService: CommonService) {
    this.host = `${environment.http}://${environment.apiUrl}`;
  }

  private getKey(): string {
    return this.commonService.getCookie(this.cookieName);
  }

  setKey(key: string): void {
    this.commonService.setCookie(this.cookieName, key);
  }

  async createGame(game: GameMetadata, key: string): Promise<GameMetadata> {

    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', 'gameKey': key },
      body: JSON.stringify(game)
    };
    const response = await fetch(`${this.host}/api/Host/createGame`, requestOptions);

    if (response.ok) {
      this.setKey(key);
      return await response.json() as GameMetadata;
    }
    return Promise.reject();
  }

  async loadQuestions(gameId: number): Promise<Game> {
    const requestOptions = {
      method: 'GET',
      headers: { 'Content-Type': 'application/json', 'gameKey': this.getKey() },
    };
    const response = await fetch(`${this.host}/api/Host/GetQuestions?gameId=${gameId}`, requestOptions);
    if (response.ok) {
      let game = await response.json() as Game;
      game.questions.sort((q1: Question, q2: Question): number => {
        if (q1.round == q2.round) {
          return q1.number - q2.number;
        }
        return q1.round - q2.round;
      })
      return game;
    }
    return Promise.reject();
  };

  async saveQuestions(game: Game): Promise<void> {
    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', 'gameKey': this.getKey() },
      body: JSON.stringify(game)
    };
    await fetch(`${this.host}/api/Host/SaveQuestions`, requestOptions);
  };

  async updateQuestion(game: GameMetadata): Promise<Question> {

    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', 'gameKey': this.getKey()},
      body: JSON.stringify(game)
    };
    const response = await fetch(`${this.host}/api/Host/setCurrentQuestion`, requestOptions);

    if (response.ok) {
      return await response.json() as Question;
    }
    return Promise.reject();
  }

  async updateState(game: GameMetadata): Promise<GameMetadata> {

    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', 'gameKey': this.getKey()},
      body: JSON.stringify(game)
    };
    const response = await fetch(`${this.host}/api/Host/setState`, requestOptions);

    if (response.ok) {
      return await response.json() as GameMetadata;
    }
    return Promise.reject();
  }

}
