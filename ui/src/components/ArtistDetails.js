import React, { Component } from 'react';
import { Link } from 'react-router';
import Album from './Album';

class ArtistDetails extends React.Component {

	constructor(props) {
    	super(props);
    	this.state = {
			artist: {},
			albums: []
		};
  	}

	componentDidMount() {
		var _this = this;
		fetch("http://localhost:8080/artist/" + this.props.params.id).then(function(response){
        if (response.status !== 200) {  
        	console.log('Looks like there was a problem. Status Code: ' +  response.status);  
          	return;  
        }

        response.json().then(function(data) {  
			_this.setState(
            	{
            		artist: data.artist,
					albums: data.albums
            	});
        });  
      }); 
	}

	render() {		
		return (
			<div>
				<p>{this.state.artist.name}</p>
				{this.state.artist.thumbUrl &&
					<img src={this.state.artist.thumbUrl}/>
				}
				<ul className="list-group">
				{					
					this.state.albums && 
					this.state.albums.map(function(album, i){
					return (
						<li className="list-group-item">
							<Album id={album.id} thumbUrl={album.thumbUrl} title={album.title} year={album.year}/>
						</li>);
					})
				}				
				</ul>
            </div>
		);
	}
}

export default ArtistDetails;