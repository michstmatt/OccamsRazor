class PlayService {
    constructor() {
        this.playerNameKey = "player-key";
        this.idKey = "player-id";
    }

    static getHost = () => "https://" + process.env.REACT_APP_API_URL;

    static async loadGames() {
        const response = await fetch(`${this.getHost()}/api/Play/LoadGames`);
        if (response.ok) {
            return response.json();
        }
        return undefined;
    };

    static async loadQuestion(gameId) {
        const response = await fetch(`${this.getHost()}/api/Play/GetCurrentQuestion?gameId=${gameId}`);
        if (response.ok) {
            return response.json();
        }
    }

    static async getState(gameId) {
        const response = await fetch(`${this.getHost()}/api/Play/GetState?gameId=${gameId}`);
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
        return await fetch(`${this.getHost()}/api/Play/submitAnswer`, requestOptions);
    }
}

export { PlayService }