import React, { Component } from 'react';

export class WaitPage extends Component {
    static displayName = WaitPage.name;

    constructor(props) {
        super(props);
        this.state = {
            gameName: this.props.gameName,
            ellipses : ''
        };
    }

    componentDidMount() {
        setInterval(()=> {
            let tmp = this.state.ellipses + '.';
            if (tmp.length >3)
                tmp = '';
            this.setState({ellipses: tmp});
            }, 1000
        );
    }


    render() {
        return (
            <div className="card">
                <h3>Waiting for {this.state.gameName} to start {this.state.ellipses}</h3>
            </div>
        );
    }


}

