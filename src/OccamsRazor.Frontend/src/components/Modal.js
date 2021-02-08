import React, { Component } from 'react';

export class Modal extends Component {
    static displayName = Modal.name;

    constructor(props) {
        super(props);
        this.state = {
            show: this.props.show,
            text: this.props.text,
        };
    }

    componentDidMount() {
    }

    closeEvent = (event) =>
    {
        this.setState({show: false});
    }

 

    renderModal(text) {
        return (
            <div id="myModal" className="modal">

            
            <div className="modal-content">
                <span className="close" onClick={this.closeEvent}>&times;</span>
                <p>{text}</p>
            </div>
    
        </div>
        );
    }

    render() {
        let contents = this.state.show
            ? this.renderModal(this.state.text)
            : <div/>;

        return (
            <div>
                {contents}
            </div>
        );
    }
}

