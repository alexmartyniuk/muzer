import React, { Component } from 'react';
import { Link } from 'react-router';

class Album extends React.Component {
	
    render() {		
		return (
			<div>
				{this.props.thumbUrl &&
					<img src={this.props.thumbUrl}/>
				}
                <Link to={"album/"+ this.props.id}>{this.props.title}</Link>({this.props.year})
            </div>
		);
	}
}

export default Album;