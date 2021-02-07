import React, { Component } from 'react';
import { HostService } from '../services/hostService';
import { NotificationService } from '../services/notificationService';
import { ToastService } from '../services/toastService';
import { HostCurrentQuestion } from './HostCurrentQuestion';
import { HostScoreAdder } from './HostScoreAdder';

export class HostScoreQuestions extends Component {
    static displayName = HostScoreQuestions.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            selectedGame: this.props.gameId,
            selectedRound: 1,
            selectedQuestion: 1,
            gameData: {},
            answers: [],
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
        let socket = new NotificationService(false, null).getSocket();
        socket.onopen = e => {
            ToastService.setConnected(true);
            socket.send("connected");
        };

        socket.onmessage = e => {
            ToastService.sendMessage("New Responses!");
            this.refreshResponses();
        };

        socket.onclose = function (e) {
            ToastService.setConnected(false);
        };
    }

    componentDidMount() {
        this.refreshResponses();
    }

    roundSelectedHandler = (event) => {
        this.setState({ selectedRound: event.target.value });
    }

    pointsChanged = answer => event => {

        this.setState({
            answers: this.state.answers.map(a => {
                if (a.round === answer.round && a.questionNumber === answer.questionNumber && a.player.name === answer.player.name) {
                    a.pointsAwarded = event.target.value * 1;
                }
                return a;
            })
        })
    }

    submitPlayerScores = (event) => {
        this.setState({ loading: true });
        HostService.updatePlayerScores(this.state.selectedGame, this.state.answers).then((response) => {
            this.refreshResponses();
        })
    }

    refreshResponses = (event) => {
        this.setState({ loading: true });
        HostService.getPlayerAnswers(this.state.selectedGame).then((answers) => {
            this.setState({ loading: false, answers: answers });
        })
    }

    deleteAnswerHandler = (event, answer) => {
        let shouldDelete = window.confirm("Are you sure you want to delete this response?");
        if (shouldDelete) {
            HostService.deleteAnswer(this.state.gameId, answer).then(() => { });
        }
    }

    handleComponentEvent = () => {
        HostService.loadQuestions(this.state.gameId).then(() => {
            this.refreshResponses();
        });
    }

    newCurrentQuestionEvent = (currentQuestion) => {
        this.setState(
            {
                selectedRound: currentQuestion.round,
                selectedQuestion: currentQuestion.number
            }
        );

    }

    renderHostScoreQuestions(answers) {
        return (
            <div className="container">

                <table className="hostScore">
                    <thead>
                        <tr>
                            <th className="hostScore">Player Name</th>
                            <th className="hostScore">Answer</th>
                            <th className="hostScore">Wager</th>
                            <th className="hostScore">Points Awarded</th>
                            <th className="hostScore">Delete Answer</th>
                        </tr>
                    </thead>
                    <tbody>
                        {answers.filter(a => a.round === this.state.selectedRound && a.questionNumber === this.state.selectedQuestion).map(answer =>
                            <tr key={JSON.stringify(answer)}>
                                <td className="hostScore">{answer.player.name}</td>
                                <td className="hostScore">{answer.answerText}</td>
                                <td className="hostScore">{answer.wager}</td>
                                <td className="hostScore">
                                    <input type="number" className="host-score-input" onChange={this.pointsChanged(answer)} value={answer.pointsAwarded} />
                                </td>
                                <td className="hostScore"><button className="host-score-button-alt" onClick={(evt) => this.deleteAnswerHandler(evt, answer)}>X</button></td>
                            </tr>)}
                    </tbody>

                </table>
            </div>

        );
    }



    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderHostScoreQuestions(this.state.answers);

        return (
            <div >

                <div className="card">
                    <HostCurrentQuestion gameId={this.state.selectedGame} newQuestionEvent={this.newCurrentQuestionEvent} />
                </div>


                <div className="card">
                    <h3>Score Responses</h3>
                    <div>
                        <button className="host-score-button" onClick={this.refreshResponses} >Check for new responses</button>
                        <button className="host-score-button" onClick={this.submitPlayerScores} >Update Scores</button>
                    </div>

                    {contents}
                    <hr />
                    <HostScoreAdder gameId={this.state.selectedGame} cRound={this.state.selectedRound} cQuestion={this.state.selectedQuestion} refresh={this.handleComponentEvent} />
                </div>
            </div>
        );
    }




}

