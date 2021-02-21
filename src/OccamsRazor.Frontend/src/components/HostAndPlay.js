import React, { Component } from 'react';
import { WaitPage } from './WaitPage';
import { Results } from './Results';
import { PreQuestion } from '../modules/PreQuestion';
import { PostQuestion } from '../modules/PostQuestion';
import { PlayService } from '../services/playService';
import { NotificationService } from '../services/notificationService';
import { ToastService } from '../services/toastService';
import { QuestionModule } from '../modules/QuestionModule';
import { AnswerModule } from '../modules/AnswerModule';
import { McAnswerModule } from '../modules/McAnswerModule';
import { HostCurrentMcQuestion } from './HostCurrentMcQuestion';

export class HostAndPlay extends Component {
    static displayName = HostAndPlay.name;

    constructor(props) {
        super(props);
        let storedState = JSON.parse(localStorage.getItem('state'));
        this.state = {
            currentQuestion: {},
            loading: true,
            player: storedState.player,
            selectedGameId: storedState.gameId,
            wager: 1,
            answer: "",
            gameState: 0,
            isMultipleChoice: false,
            game: {
                state: 0
            }
        };
        if (this.state.player === undefined || this.state.selectedGameId === undefined) {
            this.props.history.push("/play-setup");
        }

        let socket = new NotificationService(true, this.state.player.name).getSocket();
        socket.onopen = e => {
            ToastService.setConnected(true);
            socket.send("connected");
        };

        socket.onmessage = e => {
            this.checkState();
        };

        socket.onclose = function (e) {
            ToastService.setConnected(false);
        };


    }

    componentDidMount() {
        this.checkState();
    }

    renderWait() {
        return (
            <WaitPage gameName="Occams Razor" />
        )
    }

    renderPlay() {

        let form = this.state.game.isMultipleChoice
            ? <McAnswerModule game={this.state.game} player={this.state.player} currentQuestion={this.state.currentQuestion} loading={this.state.loading} />
            : <AnswerModule game={this.state.game} player={this.state.player} currentQuestion={this.state.currentQuestion} loading={this.state.loading} />;

        let question = <QuestionModule game={this.state.game} currentQuestion={this.state.currentQuestion} loading={this.state.loading} />;
        return (
            <div className="card">
                <h3>Playing as {this.state.player.name}</h3>
                {question}
                {form}

            </div>);
    }

    renderResults() {
        return (
            <div>
                <Results gameId={this.state.selectedGameId} />
            </div>
        )
    }

    render() {


        let content = {};

        if (this.state.game.state === 0) {
            content = this.renderWait();
        }
        else if (this.state.game.state === 1) {
            content = this.renderPlay();
        }
        else if (this.state.game.state === 2) {
            content = this.renderResults();
        }
        else if (this.state.game.state === 3) {
            content = (<PreQuestion gameName={this.state.gameName} />)
        }
        else if (this.state.game.state === 4) {
            content = (<PostQuestion gameName={this.state.gameName} question={this.state.currentQuestion} />)
        }

        return (
            <div>
                {content}
                <div className="card">
                    <HostCurrentMcQuestion gameId={54} />
                </div>
            </div>
        );
    }

    async checkState() {
        this.setState({ loading: true });
        const data = await PlayService.getState(this.state.selectedGameId);
        this.setState({ game: data });
        if (this.state.game.state === 1 || this.state.game.state === 4) {
            PlayService.loadQuestion(this.state.selectedGameId).then((data) => {
                this.setState({ currentQuestion: data, loading: false });
            });
        }
    }

}

