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

	componentDidMount() {
		var _this = this;
		fetch("http://localhost/tracks/" + this.props.id).then(function(response){
            if (response.status !== 200) {  
                console.log('Looks like there was a problem. Status Code: ' +  response.status);  
                return;  
            }

            response.json().then(function(data) {  
                _this.setState(
                    {
                        track: data
                    });
            });  
      }); 
	}

    
    render() {		
		return (
			<div className="alert alert-success">
                <p>{this.state.track.title}</p>				

                {this.state.track.url &&
					//<ReactPlayer url={this.state.track.url} />
                    <ReactAudioPlayer src={this.state.track.url}/>
				}                 
            </div>
		);
	}
}

export default Track;