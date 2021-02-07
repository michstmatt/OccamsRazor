import { Common } from './common';

class HostService {
    constructor() {
        this.cookieName = "host-key";
    }


    static setKey(key) {
        Common.setCookie(this.cookieName, key);
    }

    static getKey() {
        return Common.getCookie(this.cookieName);
    }

    static deleteKey() {
        Common.clearCookie(this.cookieName);
    }

    static async loadQuestions(gameId) {
        const requestOptions = {
            method: 'GET',
            headers: { 'Content-Type': 'application/json', 'gameKey': this.getKey() },
        };
        const response = await fetch(`/api/Host/GetQuestions?gameId=${gameId}`, requestOptions);
        if (response.ok) {
            return response.json();
        }
    };

    static async saveQuestions(game) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'gameKey': this.getKey() },
            body: JSON.stringify(game)
        };
        return fetch('/api/Host/SaveQuestions', requestOptions);
    };

    static async submitAnswer(answer) {

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'hostKey': this.getKey() },
            body: JSON.stringify(answer)
        };
        const response = await fetch('/api/Play/submitAnswer', requestOptions);
        return response.ok;
    }

    static async updatePlayerScores(gameId, answers) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'gameKey': this.getKey() },
            body: JSON.stringify(answers)
        };
        return fetch(`/api/Host/UpdatePlayerScores?gameId=${gameId}`, requestOptions);
    }

    static async deleteAnswer(gameId, answer) {
        const requestOptions = {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json', 'gameKey': this.getKey() },
            body: JSON.stringify(answer)
        };
        const response = await fetch(`/api/Host/DeletePlayerAnswer?gameId=${gameId}`, requestOptions);
        if (response.ok) {
            this.loadQuestions();
        }
    }
    static async getPlayerAnswers(gameId) {
        const requestOptions = {
            method: 'GET',
            headers: { 'Content-Type': 'application/json', 'gameKey': this.getKey() }
        };
        const response = await fetch(`/api/Host/GetPlayerAnswers?gameId=${gameId}`, requestOptions);
        if (response.ok) {
            return response.json();
        }
    }
    static async createGame(name, password) {
        let game =
        {
            name: name
        };

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'gameKey': password },
            body: JSON.stringify(game)
        };
        const response = await fetch('/api/Host/createGame', requestOptions);

        if (response.ok) {
            return await response.json();
        }
    }

    static async loadResults(gameId) {
        const requestOptions = {
            method: 'GET',
            headers: { 'Content-Type': 'application/json', 'gameKey': this.getKey() },
        };
        const response = await fetch(`/api/Host/GetScoredResponses?gameId=${gameId}`, requestOptions);
        if (response.ok) {
            return response.json();
        }
    }
    static async submitCurrentQuestion(newGameData) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'gameKey': this.getKey() },
            body: JSON.stringify(newGameData)
        };
        const response = await fetch('/api/Host/SetCurrentQuestion', requestOptions);
        if (response.ok) {

        }
    }
    static async setGameState(gameData) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'gameKey': this.getKey() },
            body: JSON.stringify(gameData)
        };
        const response = await fetch(`/api/Host/SetState`, requestOptions);
        return response.json();
    }
    static async checkAuth(gameId, password) {
        const requestOptions = {
            method: 'GET',
            headers: { 'Content-Type': 'application/json', 'gameKey': password },
        };
        const response = await fetch(`/api/Host/Validate?gameId=${gameId}`, requestOptions);
        if (response.ok) {
            this.setKey(password);
        }
        return response.ok;
    }

}
export { HostService };