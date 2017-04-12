import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import { Link } from 'react-router';


class App extends Component {
      
  render() {
    return (
		<ul>
			<h3>This is super puper music player!</h3>
			<Link to="search">Search</Link><br/>   
			<Link to="about">About</Link>
			
			{this.props.children}
			    
		</ul>  
    );
  }
}

export default App;
