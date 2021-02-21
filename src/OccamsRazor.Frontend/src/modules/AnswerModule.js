import React, { Component } from 'react';
import { PlayService } from '../services/playService';
import { ToastService } from '../services/toastService';

export class AnswerModule extends Component {
    static displayName = AnswerModule.name;

    constructor(props) {
        super(props);
        this.state = {
            currentQuestion: props.currentQuestion,
            loading: true,
            player: props.player,
            game: props.game,
            wager: 1,
            answer: ""
        };
    }
    componentWillReceiveProps (props){
        this.setState({currentQuestion: props.currentQuestion, game: props.game, loading: props.loading});
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
            gameId: this.state.game.gameId * 1,
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
    render() {
        return (this.state.loading
            ? (<p><em>Loading...</em></p>)
            : this.renderForm()
        );
    }
}