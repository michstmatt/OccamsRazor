import React, { Component } from 'react';

export class PostQuestionPage extends Component {
    static displayName = PostQuestionPage.name;

    constructor(props) {
        super(props);
        this.state = {
            gameName: this.props.gameName,
            answer: this.props.answer
        };
    }

    componentDidMount() {
    }


    render() {
        return (
            <div className="card">
                <h3>Correct Answer: {this.state.answer }</h3>
            </div>
        );
    }


}

