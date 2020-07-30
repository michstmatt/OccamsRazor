import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';

export class Join extends Component {
    static displayName = Join.name;

    constructor(props) {
        super(props);
    }
    
    join = (event) =>
    {
        //this.props.history.push("/play-setup");
        window.location = "/play-setup";
    }


    render() {
        return (
            <div className="card">
                <h3>Join a game</h3>
                
                <input type="button" value="play" className="host-question-submit" onClick={this.join}/>
            </div>
        );
    }
}

