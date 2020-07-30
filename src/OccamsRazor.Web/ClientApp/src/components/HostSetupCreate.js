import React, { Component } from 'react';

export class HostSetupCreate extends Component {
    static displayName = HostSetupCreate.name;

    constructor(props) {
        super(props);
        this.state = {
            name: "",
            password: "",
            gameId: 0
        };
    }

    componentDidMount() {
    }

    joinSubmitHandler = (event) => {
        event.preventDefault();
        this.createGame();
    }

    nameChangeHandler = (event) => {
        this.setState({name : event.target.value});
    }
    
    passwordChangeHandler = (event) => {
        this.setState({password : event.target.value});
    }

    renderHostSetupCreate() {
        return (
            <form className="answer-container" onSubmit={this.joinSubmitHandler}>
                <h3 className="host-join-label"><span className="secondary" >Create a Game</span></h3>
                <span>Name</span>
                <input className="answer-input" type="text" onChange={ this.nameChangeHandler } />
                <br />

                <span>Password</span>
                <input className="answer-input" type="password" onChange={ this.passwordChangeHandler } />

                <br />
                <br />
                <input type="submit" value="Create" className="answer-submit" />
            </form>
        );
    }

    render() {
        let contents = this.renderHostSetupCreate();

        return (
            <div>
                {contents}
            </div>
        );
    }

    async createGame()
    {
        let game = 
        {
            name: this.state.name
        };

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(game)
        };
        const response = await fetch('/api/Host/createGame', requestOptions);
        
        if(response.ok)
        {
            let data = await response.json();
            alert(data.gameId);
        }
    }
}

