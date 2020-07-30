import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;
  
    join = (event) =>
    {
        window.location = "/play-setup";
    }

    host = (event) =>
    {
        window.location = "/host";
    }


    render() {
        return (
            <div className="card">
                <h1 id="tabelLabel"><span className="secondary">Occams Razor Trivia</span></h1>
                <div className="card">
                    <h3>Join a game</h3>
                    <input type="button" value="Play" className="host-question-submit" onClick={this.join}/>
                </div>
                <div className="card">
                    <h3>Host a game</h3>
                    <input type="button" value="Host" className="host-question-submit" onClick={this.host}/>
                </div>
            </div>
        );
    }


}
