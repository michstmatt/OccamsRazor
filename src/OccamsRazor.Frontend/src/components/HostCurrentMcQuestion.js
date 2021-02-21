import React, { Component } from 'react';
import { isReturnStatement } from 'typescript';
import { HostService } from '../services/hostService';
import { PlayService } from '../services/playService';

export class HostCurrentMcQuestion extends Component {
    static displayName = HostCurrentMcQuestion.name;

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
        this.loadCurrentQuestion();
    }

    questionSelectedHandler = (event) => {
        let index = event.target.value;
        this.setState({ selectedQuestion: this.state.gameData.questions[index] });
    }

    updateCurrentQuestionHandler = (event) => {
        this.submitCurrentQuestion(this.state.selectedQuestion);
    }

    updateStateHandler = (event, state) => {
        this.setGameState(state);
    }

    renderPlayerStatus(answers) {
        
    }

    renderHostCurrentQuestion(questions, cQuestion, state) {

        let stateText = "";

        if (state === 1) stateText = "Question";
        if (state === 2) stateText = "Results";
        if (state === 3) stateText = "Pre Question";
        if (state === 4) stateText = "Post Question";

        return (
            <div>
                <div>
                    <h4> <span className="secondary"> Currently Showing </span>{stateText}</h4>
                    <button className={state === 3 ? "host-score-button-alt" : "host-score-button"} onClick={e => this.updateStateHandler(e, 3)} >Show Pre Question</button>
                    <button className={state === 1 ? "host-score-button-alt" : "host-score-button"} onClick={e => this.updateStateHandler(e, 1)}>Show Question</button>
                    <button className={state === 4 ? "host-score-button-alt" : "host-score-button"} onClick={e => this.updateStateHandler(e, 4)}>Show Answer</button>
                    <button className={state === 2 ? "host-score-button-alt" : "host-score-button"} onClick={e => this.updateStateHandler(e, 2)}>Show Results</button>
                </div>
                <button className="host-score-button" onClick={this.updateCurrentQuestionHandler} >Update Current Question</button>
                This will set the state to Pre-Question
            </div>

        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderHostCurrentQuestion(this.state.gameData.questions, this.state.gameData.questions[this.state.currentQuestionIndex], this.state.gameData.metadata.state);

        return (
            <div className="container">
                {contents}
            </div>
        );
    }
    async setGameState(state) {
        this.setState({ loading: true });
        let body = this.state.gameData.metadata;
        body.state = state;

        let game = this.state.gameData;
        game.metadata = await HostService.setGameState(body);
        this.setState({ loading: false, gameData: game });
    }

    async loadCurrentQuestion() {
        this.setState({ loading: true });
        if(this.state.selectedGame === undefined) return;
        const data = await HostService.loadQuestions(this.state.selectedGame);
        if(data === undefined) return;
        this.setState({ gameData: data, loading: false, currentQuestionIndex: data.metadata.currentQuestion});
    }

    async loadAnswers(){

    }

    async submitCurrentQuestion(question) {
        this.setState({ loading: true });
        let newGameData = this.state.gameData.metadata;
        newGameData.currentQuestion = question.number;
        newGameData.currentRound = question.round;
        await HostService.submitCurrentQuestion(newGameData);
        this.loadCurrentQuestion();
        this.props.newQuestionEvent(this.state.selectedQuestion);
    }

}