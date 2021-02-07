import {Common} from './common';

class PlayService {
    constructor() {
        this.playerNameKey = "player-key";
        this.idKey = "player-id";
    }

    static async loadGames() {
        const response = await fetch('/api/Play/LoadGames');
        if (response.ok) {
            return response.json();
        }
        return undefined;
    };

    static async loadQuestion(gameId) {
        const response = await fetch('/api/Play/GetCurrentQuestion?gameId=' + gameId);
        if (response.ok) {
            return response.json();
        }
    }

    static async getState(gameId) {
        const response = await fetch('/api/Play/GetState?gameId=' + gameId);
        if (response.ok) {
            return response.json();
        }
    }

    static async submitAnswer(answer) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(answer)
        };
        return await fetch('/api/Play/submitAnswer', requestOptions);
    }
}

export { PlayService }