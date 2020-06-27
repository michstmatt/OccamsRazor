import React, { Component } from 'react';

export class HostQuestions extends Component {
    static displayName = HostQuestions.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            selectedGame: 20,
            rounds: [
                {name: 'One', number:1},
                {name: 'Two', number:2},
                {name: 'Three', number:3},
                {name: 'Half Time', number:4},
                {name: 'Four', number:5},
                {name: 'Five', number:6},
                {name: 'Six', number:7},
                {name: 'Final', number:8},
            ]
        };
    }

    componentDidMount() {
        this.loadGames();
    }

    joinSubmitHandler = (event) => {
        event.preventDefault();
        localStorage.setItem('state', JSON.stringify({ player: { name: this.state.player }, gameId: 20 }));

        this.props.history.push("/play-game");
    }

    gameSelectedHandler = (event) => {
        this.setState({ selectedGame: event.target.value });
    }

    nameChangeHandler = (event) => {
        this.setState({ player: event.target.value });
    }

    renderHostQuestions(rounds, questions) {
        return (
            <div className="card">
                <form method="POST">
                    <h2>Game Name </h2>
                    <input className="host-question-input" />
                    <hr />
                    <br />
                    <label>Round</label>
                    <select class="host-score-input" ng-model="roundNum" ng-init="roundNum='1'">
                        {rounds.map(round =>
                            <option value={round.number}> { round.name }</option>
                        )}
                    </select>
                    <br />
                    <table class="hostScore">
                        <tr>
                            <th style="width:10%; margin:15px;">Number</th>
                            <th style="width:30%; margin:15px;">Category</th>
                            <th style="width:30%; margin:15px; ">Question</th>
                            <th style="width:30%; margin:15px; ">Answer</th>
                            <th style="width:10%; margin 15px;">Show</th>
                        </tr>


                    </table>
                    <br />
                    <input type="button" value="Save" ng-click="submit($parent.game)" class="host-question-submit" />
                    <br /><br />
                </form>
            </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderHostQuestions(this.state.questions, this.state.rounds);

        return (
            <div className="card">
                {this.state.player}
                <h1 id="tabelLabel" >Join a game</h1>
                {contents}
            </div>
        );
    }

    async loadQuestions() {
        const response = await fetch('/api/Host/GetQuestions?gameId=20');
        const data = await response.json();
        this.setState({ questions: data, loading: false});
    }
}

