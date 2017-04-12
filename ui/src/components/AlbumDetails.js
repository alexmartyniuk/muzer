import React, { Component } from 'react';
import { Link } from 'react-router';
import Track from './Track';

class AlbumDetails extends React.Component {

	constructor(props) {
    	super(props);
    	this.state = {
			album: {},
			tracks: []
		};
  	}

	componentDidMount() {
		var _this = this;
		fetch("http://localhost/albums/" + this.props.params.id).then(function(response){
        if (response.status !== 200) {  
        	console.log('Looks like there was a problem. Status Code: ' +  response.status);  
          	return;  
        }

        response.json().then(function(data) {  
			_this.setState(
            	{
            		album: data.album,
					tracks: data.tracks
            	});
        });  
      }); 
	}

	render() {		
		return (
			<div className="card-block">
				<p>{this.state.album.title}</p>
				{this.state.album.thumb &&
					<img className="card-img-top" src={this.state.album.thumb}/>
				}

				{this.state.tracks.map(function(track, i){
					return (
						<div>
							<Track id={track.id} title={track.title} position={track.position}/>
						</div>);
				})} 
            </div>
		);
	}
}

export default AlbumDetails;