import React, { Component } from 'react';
import { HostDeleteGame } from './HostDeleteGame';
import { HostService } from '../services/hostService';

export class HostQuestions extends Component {
    static displayName = HostQuestions.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            selectedGame: this.props.gameId,
            selectedRound: 1,
            gameData: {},
            questions: [],
            rounds: [
                { name: 'One', number: 1 },
                { name: 'Two', number: 2 },
                { name: 'Three', number: 3 },
                { name: 'Half Time', number: 4 },
                { name: 'Four', number: 5 },
                { name: 'Five', number: 6 },
                { name: 'Six', number: 7 },
                { name: 'Final', number: 8 },
            ]
        };
    }

    componentDidMount() {
        HostService.loadQuestions(this.state.selectedGame).then((data) => {
            this.setState({ gameData: data, questions: data.questions, loading: false });
        });
    }

    joinSubmitHandler = (event) => {
        event.preventDefault();
        localStorage.setItem('state', JSON.stringify({ player: { name: this.state.player }, gameId: 20 }));

        this.props.history.push("/play-game");
    }

    roundSelectedHandler = (event) => {
        this.setState({ selectedRound: event.target.value * 1 });
    }

    nameChangeHandler = (event) => {
        this.setState({ player: event.target.value });
    }

    categoryChangedHandler = (event, question) => {
        this.setState({
            questions: this.state.questions.map(q => {
                if (q.round === question.round && q.number === question.number) {
                    q.category = event.target.value;
                }
                return q;
            })
        })

    }

    questionTextChangeHandler = (event, question) => {
        this.setState({
            questions: this.state.questions.map(q => {
                if (q.round === question.round && q.number === question.number) {
                    q.text = event.target.value;
                }
                return q;
            })
        })

    }

    answerTextChangeHandler = (event, question) => {
        this.setState({
            questions: this.state.questions.map(q => {
                if (q.round === question.round && q.number === question.number) {
                    q.answerText = event.target.value;
                }
                return q;
            })
        })
    }

    saveQuestionsHandler = (evt) => {
        let game = this.state.gameData;
        game.questions = this.state.questions;
        HostService.saveQuestions(game).then((response) => { });
    }

    renderHostQuestions(rounds, questions) {
        return (
            <div className="card">
                <form method="POST">
                    <h2>Game Name </h2>
                    <input className="host-question-input" value={this.state.gameData.metadata.name} />
                    <hr />
                    <br />
                    <label>Round</label>
                    <select className="host-score-input" onChange={this.roundSelectedHandler}>
                        {rounds.map(round =>
                            <option key={round.number} value={round.number}> {round.name}</option>
                        )}
                    </select>
                    <br />
                    <table className="hostScore">
                        <thead>
                            <tr>
                                <th >Number</th>
                                <th >Category</th>
                                <th >Question</th>
                                <th >Answer</th>
                            </tr>
                        </thead>
                        <tbody>

                            {questions.filter(q => q.round === this.state.selectedRound).map(question =>
                                <tr key={`${question.round}-${question.number}`}>
                                    <td>{question.number}</td>
                                    <td><input className="host-question-input" value={question.category} onChange={(evt) => this.categoryChangedHandler(evt, question)} /></td>
                                    <td><textarea className="host-question-input" value={question.text} onChange={(evt) => this.questionTextChangeHandler(evt, question)} /></td>
                                    <td><input className="host-question-input" value={question.answerText} onChange={(evt) => this.answerTextChangeHandler(evt, question)} /></td>
                                </tr>
                            )}
                        </tbody>


                    </table>
                    <br />
                    <input type="button" value="Save" className="host-question-submit" onClick={this.saveQuestionsHandler} />
                    <br /><br />
                </form>
            </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderHostQuestions(this.state.rounds, this.state.questions);

        return (
            <div>
                {contents}
                <HostDeleteGame gameId={this.state.selectedGame} />
            </div>
        );
    }


}

