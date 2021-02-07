class ToastService {
    constructor() {
    }

    static register = (toastDiv) => {
        this.toastDiv = toastDiv;
    }

    static sendMessage = (message) => {
        if (this.toastDiv !== undefined){
            this.toastDiv.setText(message);
        }
    }

    static sendError = (message) => {
        if (this.toastDiv !== undefined){
            this.toastDiv.setText(message, true);
        }
    }

    static setConnected = (connected) => {
        if (this.toastDiv !== undefined){
            this.toastDiv.setConnected(connected);
        }
    }

}

export {ToastService};