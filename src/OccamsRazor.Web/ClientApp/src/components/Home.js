import React, { Component } from 'react';
import { Join } from './Join';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <Join />
        </div>
    );
  }
}
