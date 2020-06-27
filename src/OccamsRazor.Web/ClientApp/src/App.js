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
      </Layout>
    );
  }
}
