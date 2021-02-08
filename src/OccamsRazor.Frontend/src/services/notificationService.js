class NotificationService {
    constructor(player, name){
        const host = process.env.REACT_APP_API_URL;
        const base = `wss://${host}/notifications`;
        
        const url = (player === true) ? `${base}/player/${name}` : `${base}/host`;
        
        this.websocket = new WebSocket(url);
    }

    getSocket = () => this.websocket;
}

export {NotificationService}