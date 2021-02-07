class NotificationService {
    constructor(player, name){
        const domain = window.location.hostname;
        const port = 5001;
        const base = `wss://${domain}:${port}/notifications`;
        
        const url = (player == true) ? `${base}/player/${name}` : `${base}/host`;
        
        this.websocket = new WebSocket(url);
    }

    getSocket = () => this.websocket;
}

export {NotificationService}