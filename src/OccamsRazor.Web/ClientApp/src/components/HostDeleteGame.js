import React, { Component } from 'react';

export class HostDeleteGame extends Component {
    static displayName = HostDeleteGame.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            selectedGame: this.props.gameId,
            password: ''
        }
    }

    componentDidMount() {
    }

    deleteEvent = (event) => {
        let shouldDelete = window.confirm("Are you sure you want to delete this game and all player answers?");
        if(shouldDelete)
        {
            this.deleteGame().then( () =>
            {
                alert("This game has been deleted");
                window.location = '/';
            })
        }
    }
    passwordChangeHandler = (event) => {
        this.setState({password : event.target.value});
    }

    render() {

        return (
            <div className="card">
                <h3>Delete</h3>
                <span>Password</span>
                <input className="answer-input" type="password" onChange={ this.passwordChangeHandler } />
                <button className="host-delete" onClick={this.deleteEvent}>Delete this game?</button>
            </div>
        );
    }

    async deleteGame() {
        const requestOptions = {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json', 'gameKey' : this.state.password },
        };
        const response = await fetch(`/api/Host/DeleteGame?gameId=${this.state.selectedGame}`, requestOptions);
        if(response.ok)
        {
;
        }
        
    }



}

