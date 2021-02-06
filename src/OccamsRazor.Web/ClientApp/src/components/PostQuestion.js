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
        return (
            <div className="card">
                <h4><span className="secondary">Round</span> {this.state.question.round}  <span className="secondary">Question</span> {this.state.question.number}</h4>
                <h2><span className="primary">Question</span></h2>
                <p>{this.state.question?.text ?? ""}</p>
                <h2><span className="primary">Answer</span></h2>
                <p>{this.state.question?.answerText ?? ""}</p>
            </div>
        );
    }


}

