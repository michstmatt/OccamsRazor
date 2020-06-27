import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';

export class Join extends Component {
    static displayName = Join.name;

    render() {
        return (
            <div>
                  <NavLink tag={Link} to="/play-setup">Join A Game</NavLink>
            </div>
        );
    }
}

