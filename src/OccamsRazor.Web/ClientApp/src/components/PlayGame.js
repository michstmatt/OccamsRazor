import React, { Component } from 'react';
import { Toast } from './Toast';
import { WaitPage } from './WaitPage';
import { Results } from './Results';

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
            gameState : 0
        };
        if (this.state.player === undefined || this.state.selectedGameId === undefined)
        {
            this.props.history.push("/play-setup");
        }
    }

    componentDidMount() {
        this.checkState();
        setInterval(()=> this.checkState(), 10000);
    }

    answerSumbitHandler = (event) => {
        event.preventDefault();
        let answer = 
        {
            player: this.state.player,
            answerText: this.state.answer,
            wager: this.state.wager*1,
            questionNumber: this.state.currentQuestion.number*1,
            round: this.state.currentQuestion.round*1,
            gameId: this.state.selectedGameId *1,
        };

        if (answer.answerText !== "" && answer.wager >0 && answer.wager <7)
            this.submitAnswer(answer);
        else
            this.refs.toast.setText("You must fill out all fields!");
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

    renderPlay() 
    {
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

    renderResults()
    {
        return(
            <div>
                <Results gameId= {this.state.selectedGameId} />
            </div>
        )
    }

    render() {


        let content = {};
        
        if(this.state.gameState === 0)
        {
            content = this.renderWait();
        }
        else if(this.state.gameState === 1)
        {
            content = this.renderPlay();
        }
        else if(this.state.gameState === 2)
        {
            content = this.renderResults();
        }

        return (
            <div>
                <Toast ref="toast" />
                {content}
            </div>
        );
    }

    checkState() {
        this.getState().then( () => {
            if (this.state.gameState === 1)
            {
                this.loadQuestion();
            }
        });
    }

    async loadQuestion() {
        const response = await fetch('/api/Play/GetCurrentQuestion?gameId='+ this.state.selectedGameId);
        if (response.ok)
        {
            const data = await response.json();
            this.setState({ currentQuestion: data,loading: false });
        }
    }

    async getState(){
        const response = await fetch('/api/Play/GetState?gameId='+ this.state.selectedGameId);
        if (response.ok)
        {
            const data = await response.json();
            this.setState({gameState: data.state});
        }
    }

    async submitAnswer(answer) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(answer)
        };
        const response = await fetch('/api/Play/submitAnswer', requestOptions);
        
        if(response.ok)
        {
            this.refs.toast.setText("Your answer was received");
            this.setState({})
        }
        else
        {
            this.refs.toast.setText("There was an error, please submit again");
        }

    }
}

