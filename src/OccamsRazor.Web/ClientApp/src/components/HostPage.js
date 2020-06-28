import React, { Component } from 'react';
import { HostQuestions } from './HostQuestions';
import { HostScoreQuestions } from './HostScoreQuestions';
import { Results } from './Results';

export class HostPage extends Component {
    static displayName = HostPage.name;

    constructor(props) {
        super(props);
        this.state = {
            games: [],
            loading: true,
            player: "",
            selectedGame: 20,
            selectedTab: "Questions"
        };
    }

    componentDidMount() {
        
    }

    tabSelected = (event, tab) =>
    {
        this.setState({selectedTab: tab})
    }

    render() {

        let component;
        if (this.state.selectedTab == "Questions")
        {
            component = <HostQuestions gameId={this.state.selectedGame}/>;
        }
        if (this.state.selectedTab == "Answers")
        {
            component = <HostScoreQuestions gameId={this.state.selectedGame} />;
        }
        if (this.state.selectedTab == "Results")
        {
            component = <Results gameId={this.state.selectedGame} />;
        }


        return (
            <div className="cardLarge">
                <h2>
                    <a onClick={ (evt) => this.tabSelected(evt,"Questions")} >Edit Questions</a> |
                    <a onClick={ (evt) => this.tabSelected(evt,"Answers")} >Host Game</a> |
                    <a onClick={ (evt) => this.tabSelected(evt,"Results")} >View Results</a>
                </h2>
                {component}
            </div>
        );
    }

    async loadGames() {
        const response = await fetch('/api/Play/LoadGames');
        const data = await response.json();
        this.setState({ games: data, loading: false, selectedGame: data[0].gameId});
    }
}

