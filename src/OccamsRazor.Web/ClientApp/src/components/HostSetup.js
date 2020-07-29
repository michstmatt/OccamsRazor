import React, { Component } from 'react';

export class HostSetup extends Component {
    static displayName = HostSetup.name;

    constructor(props) {
        super(props);
        this.state = {
            games: [],
            loading: true,
            password: "",
            selectedGame: 20
        };
    }

    componentDidMount() {
        this.loadGames();
    }

    joinSubmitHandler = (event) => {
        event.preventDefault();
        localStorage.setItem('state', JSON.stringify({password: this.state.password, gameId: this.state.selectedGame}));
        this.props.history.push("/host-edit");
    }

    gameSelectedHandler = (event) =>{
        this.setState({ selectedGame : event.target.value });
    }
    
    passwordChangeHandler = (event) => {
        this.setState({password : event.target.value});
    }

    renderHostSetup(games) {
        return (
            <form className="answer-container" onSubmit={this.joinSubmitHandler}>
                <h3 className="host-join-label">Choose a Game:</h3>

                <select className="answer-input" onChange={ this.gameSelectedHandler }>
                    {games.map(game =>
                        <option key={game.gameId} value={game.gameId} >{game.name}</option>
                    )};
                </select>
                <br />

                <h3 className="host-join-label">Password: </h3>
                <input className="answer-input" type="password" onChange={ this.passwordChangeHandler } />

                <br />
                <br />
                <input type="submit" value="Join" className="answer-submit" />
            </form>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderHostSetup(this.state.games);

        return (
            <div className="card">
                <h1 id="tabelLabel" >Host a game</h1>
                {contents}
            </div>
        );
    }

    async loadGames() {
        const response = await fetch('/api/Play/LoadGames');
        const data = await response.json();
        this.setState({ games: data, loading: false, selectedGame: data[0].gameId});
    }
}

