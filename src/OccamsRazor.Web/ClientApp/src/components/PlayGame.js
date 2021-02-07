import React, { Component } from 'react';
import { WaitPage } from './WaitPage';
import { Results } from './Results';
import { PreQuestion } from './PreQuestion';
import { PostQuestion } from './PostQuestion';
import { PlayService } from '../services/playService';
import { NotificationService } from '../services/notificationService';
import { ToastService } from '../services/toastService';

export class PlayGame extends Component {
    static displayName = PlayGame.name;

    constructor(props) {
        super(props);
        let storedState = JSON.parse(localStorage.getItem('state'));
        this.state = {
            currentQuestion: {},
            loading: true,
            player: storedState.player,
            selectedGameId: storedState.gameId,
            wager: 1,
            answer: "",
            gameState: 0
        };
        if (this.state.player === undefined || this.state.selectedGameId === undefined) {
            this.props.history.push("/play-setup");
        }

        let socket = new NotificationService(true, this.state.player.name).getSocket();
        socket.onopen = e => {
            ToastService.setConnected(true);
            socket.send("connected");
        };

        socket.onmessage = e => {
            this.checkState();
        };

        socket.onclose = function (e) {
            ToastService.setConnected(false);
        };


    }

    componentDidMount() {
        this.checkState();
    }


    answerSumbitHandler = (event) => {
        event.preventDefault();
        let answer =
        {
            player: this.state.player,
            answerText: this.state.answer,
            wager: this.state.wager * 1,
            questionNumber: this.state.currentQuestion.number * 1,
            round: this.state.currentQuestion.round * 1,
            gameId: this.state.selectedGameId * 1,
        };

        if (answer.answerText !== "" && answer.wager > 0 && answer.wager < 7) {
            ToastService.sendMessage("Loading...");
            PlayService.submitAnswer(answer).then(() => {
                ToastService.sendMessage(`Your answer for R${answer.round} Q${answer.questionNumber} was received`);
            }).catch((err) => {
                ToastService.sendError("An error occured. Please submit again");
            });
        }
        else {
            ToastService.sendError("You must fill out all fields!");
        }
    }

    wagerChangeHandler = (event) => {
        this.setState({ wager: event.target.value });
    }

    answerChangeHandler = (event) => {
        this.setState({ answer: event.target.value });
    }


    renderQuestion(games) {
        return (
            <div className="card">
                <h4><span className="secondary">Round</span> {this.state.currentQuestion.round}  <span className="secondary">Question</span> {this.state.currentQuestion.number}</h4>
                <h2><span className="primary">Category</span></h2>{this.state.currentQuestion.category}
                <h2><span className="primary">Question</span></h2>{this.state.currentQuestion.text}
            </div>
        );
    }
    renderForm() {
        return (
            <div className="card">
                <form method="POST" className="answer-container" onSubmit={this.answerSumbitHandler}>
                    <label>Answer</label>
                    <br />
                    <input name="AnswerText" className="answer-input" type="text" onChange={this.answerChangeHandler} />
                    <br /><br />

                    <label>Wager</label>
                    <br />
                    <input type="number" className="answer-input" min="1" max="6" onChange={this.wagerChangeHandler} />
                    <br /><br />
                    <input type="submit" className="answer-submit" />
                </form>
            </div>
        );
    }

    renderWait() {
        return (
            <WaitPage gameName="Occams Razor" />
        )
    }

    renderPlay() {
        let question = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderQuestion(this.state.currentQuestion);
        return (
            <div className="card">
                <h3>Playing as {this.state.player.name}</h3>
                {question}
                {this.renderForm()}

            </div>)
    }

    renderResults() {
        return (
            <div>
                <Results gameId={this.state.selectedGameId} />
            </div>
        )
    }

    render() {


        let content = {};

        if (this.state.gameState === 0) {
            content = this.renderWait();
        }
        else if (this.state.gameState === 1) {
            content = this.renderPlay();
        }
        else if (this.state.gameState === 2) {
            content = this.renderResults();
        }
        else if (this.state.gameState === 3) {
            content = (<PreQuestion gameName={this.state.gameName} />)
        }
        else if (this.state.gameState === 4) {
            content = (<PostQuestion gameName={this.state.gameName} question={this.state.currentQuestion} />)
        }

        return (
            <div>
                {content}
            </div>
        );
    }

    checkState() {
        this.setState({ loading: true });
        PlayService.getState(this.state.selectedGameId).then((data) => {
            this.setState({ gameState: data.state });
            if (this.state.gameState === 1 || this.state.gameState === 4) {
                PlayService.loadQuestion(this.state.selectedGameId).then((data) => {
                    this.setState({ currentQuestion: data, loading: false });
                });
            }
        });
    }

}

