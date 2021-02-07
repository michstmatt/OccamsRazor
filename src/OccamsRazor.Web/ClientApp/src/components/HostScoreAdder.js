import React, { Component } from 'react';
import { HostService } from '../services/hostService';

export class HostScoreAdder extends Component {
    static displayName = HostScoreAdder.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            selectedGame: this.props.gameId,
            selectedRound: this.props.cRound,
            selectedQuestion: this.props.cQuestion,
            playerName: "",
            answerText: "",
            wager: 0
        };
    }

    componentWillReceiveProps(props) {
        this.setState(
            {
                selectedRound: props.cRound,
                selectedQuestion: props.cQuestion
            }
        );
    }

    componentDidMount() {
        this.setState();
    }

    playerNameChanged = (event) => {
        this.setState({ playerName: event.target.value })
    }

    answerTextChanged = (event) => {
        this.setState({ answerText: event.target.value })
    }

    wagerChanged = (event) => {
        this.setState({ wager: event.target.value * 1 })
    }

    formSubmit = (event) => {
        event.preventDefault();
        let answer = {
            player: { name: this.state.playerName },
            answerText: this.state.answerText,
            questionNumber: this.state.selectedQuestion * 1,
            round: this.state.selectedRound * 1,
            gameId: this.state.selectedGame * 1,
            wager: this.state.wager
        }
        const ok = HostService.submitAnswer(answer);
        if (ok) {
            this.setState({
                playerName: "",
                wager: 0,
                answerText: ""
            });

            this.props.refresh();
        }
    }

    render() {
        return (
            <div className="container">
                <h3>Add a Player's Response</h3>
                <form onSubmit={this.formSubmit}>
                    <label>Player Name</label>
                    <input className="host-question-input" onChange={this.playerNameChanged} value={this.state.playerName} />
                    <label>Answer Text</label>
                    <input className="host-question-input" onChange={this.answerTextChanged} value={this.state.answerText} />
                    <label>Wager</label>
                    <input className="host-question-input" onChange={this.wagerChanged} type="number" value={this.state.wager} />
                    <input type="submit" className="host-score-button" value="Add Answer" />
                </form>
            </div>
        );
    }


}

