import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';

import './custom.css'
import { PlaySetup } from './components/PlaySetup';
import { PlayGame } from './components/PlayGame';
import { HostQuestions } from './components/HostQuestions';
import { HostScoreQuestions } from './components/HostScoreQuestions';
import { HostPage } from './components/HostPage';
import { HostSetup } from './components/HostSetup';
import { WaitPage } from './components/WaitPage';
import { HostAndPlay } from './components/HostAndPlay';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route exact path='/play-setup' component={PlaySetup} />
        <Route exact path='/play-game' component={PlayGame} />
        <Route exact path='/host-game-questions' component={HostQuestions} />
        <Route exact path='/host-score-questions' component={HostScoreQuestions} />
        <Route exact path='/host-edit' component={HostPage} />
        <Route exact path='/host-setup' component={HostSetup} />
        <Route exact path='/host-play' component={HostAndPlay} />
        <Route exact path='/play-waiting' component={WaitPage} />
      </Layout>
    );
  }
}
