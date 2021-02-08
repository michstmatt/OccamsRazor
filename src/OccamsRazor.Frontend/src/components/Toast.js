import React, { Component } from 'react';
import {ToastService} from '../services/toastService';
export class Toast extends Component {
    static displayName = Toast.name;

    constructor(props) {
        super(props);
        this.state = {
            timeout: 1500,
            connected: false,
            error: false
        };
        ToastService.register(this);
    }

    componentDidMount() 
    {
    }

    setText = (text, error = false) => 
    {
        this.setState({text: text, show: true, error: error});
        //setTimeout( ()=> this.setState({show: false}), this.state.timeout);
    }
 

    setConnected = (connected) => this.setState({connected:connected});

    render() {
        return (
            <div className="card">
                <p>{this.state.connected ? "Connected" : "Disconnected"}</p>
                <p className={this.state.error ? "toast-error": "" }>{this.state.show ? this.state.text : "" }</p>
            </div>
        );
    }
}

