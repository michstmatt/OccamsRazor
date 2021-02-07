import React, { Component } from 'react';
import { HostService } from '../services/hostService';

export class HostSetupCreate extends Component {
    static displayName = HostSetupCreate.name;

    constructor(props) {
        super(props);
        this.state = {
            name: "",
            password: "",
            gameId: 0,
        };
    }

    componentDidMount() {
    }

    joinSubmitHandler = (event) => {
        event.preventDefault();
        HostService.createGame(this.state.name, this.state.password).then((data) => {
            localStorage.setItem('state', JSON.stringify({ password: this.state.password, gameId: data.gameId }));
            this.props.navigate()
        });
    }

    nameChangeHandler = (event) => {
        this.setState({ name: event.target.value });
    }

    passwordChangeHandler = (event) => {
        this.setState({ password: event.target.value });
    }

    renderHostSetupCreate() {
        return (
            <form className="answer-container" onSubmit={this.joinSubmitHandler}>
                <h3 className="host-join-label"><span className="secondary" >Create a Game</span></h3>
                <span>Name</span>
                <input className="answer-input" type="text" onChange={this.nameChangeHandler} />
                <br />

                <span>Password</span>
                <input className="answer-input" type="password" onChange={this.passwordChangeHandler} />

                <br />
                <br />
                <input type="submit" value="Create" className="answer-submit" />
            </form>
        );
    }

    render() {
        let contents = this.renderHostSetupCreate();

        return (
            <div>
                {contents}
            </div>
        );
    }
}

