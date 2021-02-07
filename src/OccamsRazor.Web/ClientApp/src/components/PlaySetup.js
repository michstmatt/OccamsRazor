import React, { Component } from 'react';
import { PlayService } from '../services/playService';
import { ToastService } from '../services/toastService';

export class PlaySetup extends Component {
    static displayName = PlaySetup.name;

    constructor(props) {
        super(props);
        this.state = {
            games: [],
            loading: true,
            player: "",
            selectedGame: {}
        };
    }

    componentDidMount() {
        PlayService.loadGames().then((data) => {
            this.setState({ games: data, loading: false, selectedGame: data[0].gameId });
        });
    }

    joinSubmitHandler = (event) => {
        event.preventDefault();
        if (this.state.player !== "") {
            localStorage.setItem('state', JSON.stringify({ player: { name: this.state.player }, gameId: this.state.selectedGame }));
            this.props.history.push("/play-game");
        }
        else {
            ToastService.sendMessage("Please type in a name");
        }
    }

    gameSelectedHandler = (event) => {
        this.setState({ selectedGame: event.target.value });
    }

    nameChangeHandler = (event) => {
        this.setState({ player: event.target.value });
    }

    renderPlaySetup(games) {
        return (
            <form className="answer-container" onSubmit={this.joinSubmitHandler}>
                <h3 className="host-join-label">Choose a Game:</h3>

                <select className="answer-input" onChange={this.gameSelectedHandler}>
                    {games.map(game =>
                        <option key={game.gameId} value={game.gameId} >{game.name}</option>
                    )};
                </select>
                <br />

                <h3 className="host-join-label">Name: </h3>
                <input className="answer-input" type="text" onChange={this.nameChangeHandler} />

                <br />
                <br />
                <input type="submit" value="Join" className="answer-submit" />
            </form>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderPlaySetup(this.state.games);

        return (
            <div className="card">
                <h1 id="tabelLabel" >Join a game</h1>
                {contents}
            </div>
        );
    }
}
