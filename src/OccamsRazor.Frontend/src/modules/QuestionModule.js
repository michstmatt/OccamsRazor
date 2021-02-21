import React, { Component } from 'react';

export class QuestionModule extends Component {
    static displayName = QuestionModule.name;

    constructor(props) {
        super(props);
        this.state = {
            currentQuestion: props.currentQuestion,
            game: props.game,
            loading: true
        };
    }
    componentWillReceiveProps (props){
        this.setState({currentQuestion: props.currentQuestion, game: props.game, loading: props.loading});
    }

    renderQuestion(game, question){
        return (
            <div className="card">
                <h4><span className="secondary">Round</span> {game.currentRound}  <span className="secondary">Question</span> {game.currentQuestion}</h4>
                <h2><span className="primary">Category</span></h2>{question.category}
                <h2><span className="primary">Question</span></h2>{question.text}
            </div>
        );
    }

    render(){
        return( this.state.loading 
            ? (<p><em>Loading...</em></p>)
            : this.renderQuestion(this.state.game, this.state.currentQuestion)
        );
    }

}