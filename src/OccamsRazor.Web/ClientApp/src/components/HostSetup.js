import React, { Component } from 'react';
import { HostSetupCreate } from './HostSetupCreate';

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

    navigateToHostPage = () =>
    {
        this.props.history.push("/host-edit");
    }

    joinSubmitHandler = (event) => {
        event.preventDefault();
        this.checkAuth();
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
                <h3><span className="secondary">Choose a Game</span></h3>
                <span>Name</span>
                <select className="answer-input" onChange={ this.gameSelectedHandler }>
                    {games.map(game =>
                        <option key={game.gameId} value={game.gameId} >{game.name}</option>
                    )};
                </select>
                <br />

                <span>Password</span>
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
                <h1 id="tabelLabel"><span className="primary">Host a Game</span></h1>
                <div className="card">
                    {contents}
                </div>
                <div className="card">
                    <HostSetupCreate navigate={this.navigateToHostPage}/>
                </div>
            </div>
        );
    }

    async loadGames() {
        const response = await fetch('/api/Play/LoadGames');
        const data = await response.json();
        this.setState({ games: data, loading: false, selectedGame: data[0].gameId});
    }

    async checkAuth()
    {
        const requestOptions = {
            method: 'GET',
            headers: { 'Content-Type': 'application/json', 'gameKey' : this.state.password},
        };
        const response = await fetch(`/api/Host/Validate?gameId=${this.state.selectedGame}`, requestOptions);
        if (response.ok)
        {
            localStorage.setItem('state', JSON.stringify({password: this.state.password, gameId: this.state.selectedGame}));
            this.navigateToHostPage();
        }
    }
}

