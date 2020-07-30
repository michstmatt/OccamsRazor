import React, { Component } from 'react';

export class HostGameState extends Component {
    static displayName = HostGameState.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            selectedGame: props.gameId,
            password: this.props.password,
            gameData: {},
            states: ["Paused", "Playing", "Showing Results"],
            playPauseText: ""
        };
    }

    componentDidMount() {
        this.setState();
        this.loadCurrentQuestion();
    }

    setPlayPauseText = (gamemetadata) => 
    {
        let text = (gamemetadata.state == "0") ? "Play" : "Pause";
        let gameData = this.state.gameData;
        gameData.metadata = gamemetadata;
        this.setState({gameData: gameData, playPauseText: text, loading:false})
    }

    onPlayPauseClicked = (event) =>
    {
        if (this.state.gameData.metadata.state == 1)
        {
            this.setGameState(0)
        }
        else
        {
            this.setGameState(1);
        }
    }

    onResultsClicked = (event) =>
    {
        this.setGameState(2);
    }

    render() {
            let gameState = this.state.loading ? "Loading"
              : this.state.states[this.state.gameData.metadata.state];
        return (
                <div>
                    <h3> <span className="secondary">Current Game State</span> {gameState}</h3>
                    <button className="host-score-button" onClick={this.onPlayPauseClicked} >{this.state.playPauseText}</button>
                    <button className="host-score-button" onClick={this.onResultsClicked} >Show Results</button>
                </div>
        );
    }

    async loadCurrentQuestion() {
        this.setState({loading:true});
        const requestOptions = {
            method: 'GET',
            headers: { 'Content-Type': 'application/json', 'gameKey' : this.state.password},
        };
        const response = await fetch(`/api/Host/GetQuestions?gameId=${this.state.selectedGame}`, requestOptions);
        const data = await response.json();
        this.setPlayPauseText(data.metadata);
    }

    async setGameState(state)
    {
        this.setState({loading:true});
        let newGameData = this.state.gameData.metadata;
        newGameData.state = state;

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'gameKey' : this.state.password},
            body: JSON.stringify(newGameData)
        };
        const response = await fetch('/api/Host/SetState', requestOptions);
        const data = await response.json();
        this.setPlayPauseText(data);
    }


}

