import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import { Toast } from './Toast';

export class Layout extends Component {
  static displayName = Layout.name;

  render () {
    return (
      <div>
        <NavMenu />
        <Toast />
        <Container>
          {this.props.children}
        </Container>
      </div>
    );
  }
}
