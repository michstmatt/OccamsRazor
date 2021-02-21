import React, { Component } from 'react';

export class PostQuestion extends Component {
    static displayName = PostQuestion.name;

    constructor(props) {
        super(props);
        this.state = {
            gameName: this.props.gameName,
            question: this.props.question
        };
    }

    componentDidMount() {
    }

    componentWillReceiveProps (props){
        this.setState({question: props.question});
    }

    render() {
        let question = this.state.question;
        let answerText = "";
        if (question?.answerText !== undefined) answerText = question.answerText;
        if (question?.possibleAnswers !== undefined && question?.answerId !== undefined)
                    answerText = question.possibleAnswers[question.answerId];

        return (
            <div className="card">
                <h4><span className="secondary">Round</span> {question.round}  <span className="secondary">Question</span> {question.number}</h4>
                <h2><span className="primary">Question</span></h2>
                <p>{question?.text ?? ""}</p>
                <h2><span className="primary">Answer</span></h2>
                <p>{answerText}</p>
            </div>
        );
    }


}

