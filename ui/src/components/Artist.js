import React, { Component } from 'react';
import { Link } from 'react-router';

class Artist extends React.Component {
	render() {		
		return (
			<div>
				{this.props.thumbUrl &&
					<img src={this.props.thumbUrl}/>
				}
				<Link to={"artist/"+ this.props.id}>{this.props.name}</Link>
            </div>
		);
	}
}

export default Artist;