import React, { Component } from 'react';

export class HostCurrentQuestion extends Component {
    static displayName = HostCurrentQuestion.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            selectedGame: this.props.gameId,
            password: this.props.password,
            selectedQuestion: {},
            questions: [],
            currentQuestionIndex: 0,
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
        this.loadCurrentQuestion();
    }

    questionSelectedHandler = (event) => {
        let index = event.target.value;
        this.setState({ selectedQuestion: this.state.gameData.questions[index]});
    }

    updateCurrentQuestionHandler = (event) => {
        this.submitCurrentQuestion(this.state.selectedQuestion);
    }

    renderHostCurrentQuestion(questions, cQuestion) {
        return (
            <div>
    
            <h4> <span className="secondary"> Current Round</span>{cQuestion.round} </h4>
            <h4> <span className="secondary"> Current Question</span>{cQuestion.number} </h4>
            <h4> <span className="primary"> Category </span>{cQuestion.category} </h4>
            <h4> <span className="primary"> Question </span></h4>
            <p>{cQuestion.text} </p>
            <h4> <span className="primary"> Answer </span>{cQuestion.answerText} </h4>
            <select className="host-score-input" onChange={this.questionSelectedHandler} defaultValue={this.state.currentQuestionIndex}>
                {questions.map( (question, index) =>
                    <option value={index}>R { this.state.rounds.find(q => q.number == question.round).name} Q {question.number}</option>
                )}
            </select>
            <button className="host-score-button" onClick={this.updateCurrentQuestionHandler} >Update Current Question</button>
        </div>

        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderHostCurrentQuestion(this.state.gameData.questions, this.state.gameData.questions[this.state.currentQuestionIndex]);

        return (
            <div className="container">
                {contents}
            </div>
        );
    }

    async loadCurrentQuestion() {
        this.setState({loading:true});
        const requestOptions = {
            method: 'GET',
            headers: { 'Content-Type': 'application/json', 'gameKey' : this.state.password },
        };
        const response = await fetch(`/api/Host/GetQuestions?gameId=${this.state.selectedGame}`, requestOptions);
        const data = await response.json();
        const questionIndex = data.questions.findIndex( q => q.round == data.metadata.currentRound && q.number == data.metadata.currentQuestion);
        this.setState({ gameData: data, loading: false, currentQuestionIndex: questionIndex});
        
    }

    async submitCurrentQuestion(question) {
        this.setState({loading:true});
        let newGameData = this.state.gameData.metadata;
        newGameData.currentQuestion = question.number;
        newGameData.currentRound = question.round;
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'gameKey' : this.state.password},
            body: JSON.stringify(newGameData)
        };
        const response = await fetch('/api/Host/SetCurrentQuestion', requestOptions);
        const data = await response.json();
        this.loadCurrentQuestion();
        this.props.newQuestionEvent(this.state.selectedQuestion);
    }

}

