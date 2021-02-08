class NotificationService {
    constructor(player, name){
        const host = process.env.API_URL;
        const port = 5001;
        const base = `wss://${host}:${port}/notifications`;
        
        const url = (player === true) ? `${base}/player/${name}` : `${base}/host`;
        
        this.websocket = new WebSocket(url);
    }

    getSocket = () => this.websocket;
}

export {NotificationService}