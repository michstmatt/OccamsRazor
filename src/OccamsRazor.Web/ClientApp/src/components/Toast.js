import React, { Component } from 'react';

export class Toast extends Component {
    static displayName = Toast.name;

    constructor(props) {
        super(props);
        this.state = {
            show: this.props.show,
            text: this.props.text,
            timeout: 3000,
            expireTime: -1
        };
    }

    componentDidMount() 
    {
    }

    setText = (text) => 
    {
        this.setState({text: text, show: true, expireTime: Date.now() + this.state.timeout});
        setTimeout( this.checkTimedOut, this.state.timeout);
    }

    checkTimedOut = () => {
        this.setState({show: Date.now() < this.state.expireTime});
    }
 

    renderToast(text) {
        return (
            <div className="card">
                <p>{text}</p>
            </div>
        );
    }

    render() {
        let contents = this.state.show
            ? this.renderToast(this.state.text)
            : <div/>;

        return (
            <div>
                {contents}
            </div>
        );
    }
}

