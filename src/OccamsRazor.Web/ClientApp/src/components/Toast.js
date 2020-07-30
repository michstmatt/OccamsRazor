import React, { Component } from 'react';

export class Toast extends Component {
    static displayName = Toast.name;

    constructor(props) {
        super(props);
        this.state = {
            show: this.props.show,
            text: this.props.text,
            timeout: 1500
        };
    }

    componentDidMount() 
    {
    }

    setText = (text) => 
    {
        this.setState({text: text, show: true});
        setTimeout( ()=> this.setState({show: false}), this.state.timeout);
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

