import React, { Component } from 'react';
import { PlayService } from '../services/playService';
import { ToastService } from '../services/toastService';

export class McAnswerModule extends Component {
    static displayName = McAnswerModule.name;

    constructor(props) {
        super(props);
        this.state = {
            currentQuestion: props.currentQuestion,
            loading: true,
            player: props.player,
            game: props.game,
            answer: "",
        };
    }
    componentWillReceiveProps (props){
        this.setState({currentQuestion: props.currentQuestion, game: props.game, loading: props.loading});
    }

    mcAnswerSumbitHandler = (event) => {
        event.preventDefault();
        let answer =
        {
            player: this.state.player,
            answerId: this.state.answer,
            questionNumber: this.state.currentQuestion.number * 1,
            round: this.state.currentQuestion.round * 1,
            gameId: this.state.game.gameId * 1,
        };

        if (answer.answerText !== "") {
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

    answerChangeHandler = (event) => {
        this.setState({ answer: event.target.value });
    }

    renderMcForm(possibleAnswers) {
        return (
            <div className="card">
                <form method="POST" className="answer-container" onSubmit={this.mcAnswerSumbitHandler}>
                    {Object.keys(possibleAnswers).map(k =>
                        <label class="answer-input" for={k}>
                            <input type="radio" id={k} name="answerOption" value={k} onChange={this.mcAnswerChangeHandler}></input>
                            {possibleAnswers[k]}
                            <br />
                        </label>
                    )}
                    <input type="submit" className="answer-submit" />
                </form>
            </div>
        );
    }
    render() {
        return (this.state.loading
            ? (<p><em>Loading...</em></p>)
            : this.renderMcForm(this.state.currentQuestion.possibleAnswers)
        );
    }
}