import React, { Component } from 'react';
import { HostService } from '../services/hostService';

export class Results extends Component {
    static displayName = Results.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            selectedGame: this.props.gameId,
            password: this.props.password,
            selectedRound: 1,
            playerScores: []
        };
    }

    componentDidMount() {
        this.setState({ loading: true });
        HostService.loadResults(this.state.selectedGame, this.state.password).then((data) => {
            this.setState({ loading: false, playerScores: data });
        });

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

}

