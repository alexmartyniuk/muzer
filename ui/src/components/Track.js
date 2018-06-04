import React, { Component } from 'react';
import { Link } from 'react-router';
import ReactPlayer from 'react-player';
import ReactAudioPlayer from 'react-audio-player';

class Track extends React.Component {
	
    constructor(props) {
    	super(props);
    	this.state = {
			track: {}
		};
  	}

    
    render() {		
		return (
			<div className="alert alert-success">
                <p>{this.props.position} : {this.props.title}</p>				

                {this.props.url &&
                    <ReactAudioPlayer src={this.props.url}/>
				}                 
            </div>
		);
	}
}

export default Track;