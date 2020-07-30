import React, { Component } from 'react';
import { HostCurrentQuestion } from './HostCurrentQuestion';
import { HostScoreAdder } from './HostScoreAdder';
import { HostGameState } from './HostGameState';

export class HostScoreQuestions extends Component {
    static displayName = HostScoreQuestions.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            selectedGame: this.props.gameId,
            password: this.props.password,
            selectedRound: 1,
            selectedQuestion: 1,
            gameData: {},
            answers: [],
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
        this.loadQuestions();
    }

    roundSelectedHandler = (event) => {
        this.setState({ selectedRound: event.target.value });
    }

    pointsChanged = answer => event => {
        
        this.setState({
            answers : this.state.answers.map( a => 
                {
                    if (a.round == answer.round && a.questionNumber == answer.questionNumber && a.player.name == answer.player.name)
                    {
                        a.pointsAwarded = event.target.value * 1;
                    }
                    return a;
                })
        })
    }

    submitPlayerScores = (event) => {
        this.submitAnswers(this.state.answers);
    }

    refreshResponses = (event) => this.loadQuestions();

    deleteAnswerHandler = (event, answer) =>
    {
        let shouldDelete = window.confirm("Are you sure you want to delete this response?");
        if(shouldDelete)
        {
            this.deleteAnswer(answer);
        }
    }

    handleComponentEvent = () =>
    {
        this.loadQuestions();
    }

    newCurrentQuestionEvent = (currentQuestion) =>
    {
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
                {answers.filter(a => a.round == this.state.selectedRound && a.questionNumber == this.state.selectedQuestion).map( answer =>
                <tr key={JSON.stringify(answer)}>
                    <td className="hostScore">{answer.player.name}</td>
                    <td className="hostScore">{answer.answerText}</td>
                    <td className="hostScore">{answer.wager}</td>
                    <td className="hostScore">
                        <input type="number" className="host-score-input" onChange={ this.pointsChanged(answer) } value={answer.pointsAwarded}/>
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
            <div className="card">
                <HostGameState password={this.state.password} gameId={this.state.selectedGame}/>
                <HostCurrentQuestion password={this.state.password} gameId={this.state.selectedGame} newQuestionEvent={this.newCurrentQuestionEvent} />
                <hr />

                <div>
                    <h3>Score Responses</h3>
                    <button className="host-score-button" onClick={this.refreshResponses} >Check for new responses</button>
                    <button className="host-score-button" onClick={this.submitPlayerScores} >Update Scores</button>
                </div>

                {contents}
                <hr/>
                <HostScoreAdder password={this.state.password} gameId={this.state.selectedGame} cRound={this.state.selectedRound} cQuestion={this.state.selectedQuestion} refresh={this.handleComponentEvent}/>
            </div>
        );
    }

    async loadQuestions() {
        this.setState({loading:true});
        const requestOptions = {
            method: 'GET',
            headers: { 'Content-Type': 'application/json', 'gameKey' : this.state.password }
        };
        const response = await fetch(`/api/Host/GetPlayerAnswers?gameId=${this.state.selectedGame}`, requestOptions);
        if (response.ok)
        {
            const data = await response.json();
            this.setState({ answers: data, loading: false});
        }
    }

    async submitAnswers(answers) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'gameKey' : this.state.password },
            body: JSON.stringify(answers)
        };
        const response = await fetch(`/api/Host/UpdatePlayerScores?gameId=${this.state.selectedGame}`, requestOptions);
        if (response.ok)
        {
            const data = await response.json();
        }
    }

    async deleteAnswer(answer)
    {
        const requestOptions = {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json', 'gameKey' : this.state.password },
            body: JSON.stringify(answer)
        };
        const response = await fetch(`/api/Host/DeletePlayerAnswer?gameId=${this.state.selectedGame}`, requestOptions);
        if (response.ok)
        {
            const data = await response.json();
            this.loadQuestions(); 
        }
    }
}

