import React, { Component } from 'react';

export class Results extends Component {
    static displayName = Results.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            selectedGame: this.props.gameId,
            selectedRound: 1,
            playerScores: []
        };
    }

    componentDidMount() {
        this.loadResults();
    }


    renderResults(results) {
        return (
            <div>
            <div className="card">
            <h3><span className="primary">Summary</span></h3>
            <table className="playerScores">
                <tr>
                    <th className="playerScores">Name</th>
                    <th className="playerScores">Score</th>
                </tr>
                {results.map(playerData =>
                <tr>
                    <td className="playerScores">{playerData.player.name}</td>
                    <td className="playerScores">{playerData.totalScore}</td>
                </tr>
                )}
            </table>
        </div>
        <br />

        <div className="card">
            <h3> <span className="primary">Score Breakdown</span></h3>
            <table className="playerScores">
                <tr >
                    <td className="playerScores">Question</td>
                    {results[0].playerAnswers.map( result =>
                        <td className="playerScores"> R {result.round} Q{result.questionNumber}
                    </td>
                    )}
                </tr>
                {results.map(playerData => 
                <tr  ng-repeat="playerData in results">
                    <td className="playerScores">{playerData.player.name}</td>
                    {playerData.playerAnswers.map( result =>
                    <td className="playerScores"> {result.pointsAwarded}
                    </td>
                    )}
                </tr>
                )}
            </table>
        </div>
        </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderResults(this.state.playerScores);

        return (
            <div>
                {contents}
            </div>
        );
    }

    async loadResults() {
        const response = await fetch(`/api/Host/GetScoredResponses?gameId=${this.state.selectedGame}`);
        const data = await response.json();
        this.setState({ playerScores: data, loading: false});
    }
}

