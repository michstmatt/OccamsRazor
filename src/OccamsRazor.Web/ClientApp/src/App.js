import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';

import './custom.css'
import { PlaySetup } from './components/PlaySetup';
import { PlayGame } from './components/PlayGame';
import { HostQuestions } from './components/HostQuestions';
import { HostScoreQuestions } from './components/HostScoreQuestions';
import { HostPage } from './components/HostPage';
import { WaitPage } from './components/WaitPage';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data' component={FetchData} />
        <Route path='/play-setup' component={PlaySetup} />
        <Route path='/play-game' component={PlayGame} />
        <Route path='/host-game-questions' component={HostQuestions} />
        <Route path='/host-score-questions' component={HostScoreQuestions} />
        <Route path='/host' component={HostPage} />
        <Route path="/play-waiting" component={WaitPage} />
      </Layout>
    );
  }
}
