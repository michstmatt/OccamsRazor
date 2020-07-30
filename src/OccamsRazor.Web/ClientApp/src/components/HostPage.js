import React, { Component } from 'react';
import { HostQuestions } from './HostQuestions';
import { HostScoreQuestions } from './HostScoreQuestions';
import { Results } from './Results';

export class HostPage extends Component {
    static displayName = HostPage.name;

    constructor(props) {
        super(props);
        let storedState = JSON.parse(localStorage.getItem('state'));
        this.state = {
            games: [],
            loading: true,
            selectedGame: storedState.gameId,
            password: storedState.password,
            selectedTab: "Questions"
        };

        if(this.state.selectedGame === undefined || this.state.selectedGame === undefined)
        {
            this.props.history.push("/host");
        }
        
    }

    componentDidMount() {
        
    }

    tabSelected = (event, tab) => {
        this.setState({ selectedTab: tab })
    }

    render() {

        let component;
        if (this.state.selectedTab === "Questions") {
            component = <HostQuestions password={this.state.password} gameId={this.state.selectedGame} />;
        }
        if (this.state.selectedTab === "Answers") {
            component = <HostScoreQuestions password={this.state.password} gameId={this.state.selectedGame} />;
        }
        if (this.state.selectedTab === "Results") {
            component = <Results password={this.state.password} gameId={this.state.selectedGame} />;
        }


        return (
            <div className="cardLarge">
                <h2>
                    <span className={this.state.selectedTab === "Questions"? "secondary": "" }>
                    <span onClick={(evt) => this.tabSelected(evt, "Questions")} >Edit Questions</span>
                    </span>
                    |
                    <span className={this.state.selectedTab === "Answers"? "secondary": "" }>
                    <span onClick={(evt) => this.tabSelected(evt, "Answers")} >Host Game</span>
                    </span>
                    |
                    <span className={this.state.selectedTab === "Results"? "secondary": "" }>
                    <span onClick={(evt) => this.tabSelected(evt, "Results")} >View Results</span>
                    </span>
                </h2>
                {component}
            </div>
        );
    }
}

