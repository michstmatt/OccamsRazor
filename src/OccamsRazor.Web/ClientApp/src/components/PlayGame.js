import React, { Component } from 'react';
import { Modal } from './Modal';

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
            showMessage: false
        };
    }

    componentDidMount() {
        this.loadQuestion();
    }

    answerSumbitHandler = (event) => {
        event.preventDefault();
        let answer = 
        {
            player: this.state.player,
            answerText: this.state.answer,
            wager: this.state.wager*1,
            questionNumber: this.state.currentQuestion.number,
            roundNumber: this.state.currentQuestion.number,
            gameId: this.state.selectedGameId,
        }
        this.submitAnswer(answer);
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

    render() {
        let question = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderQuestion(this.state.currentQuestion);



        return (
            <div className="card">
                {this.state.player.name}
                {question}
                {this.renderForm()}
                <Modal show={this.state.showMessage} text="Your Answer has been received" />
            </div>
        );
    }

    async loadQuestion() {
        const response = await fetch('/api/Play/GetCurrentQuestion?gameId='+ this.state.selectedGameId);
        const data = await response.json();
        this.setState({ currentQuestion: data,loading: false });
    }

    async submitAnswer(answer) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(answer)
        };
        const response = await fetch('/api/Play/submitAnswer', requestOptions);
       // const data = await response.json();

       this.setState({showMessage:true});

    }
}

