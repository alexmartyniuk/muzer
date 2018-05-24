import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import { Link } from 'react-router';
import Artist from './Artist';


class Search extends Component {
  
  constructor(props) {
    super(props);

    this.state = {
      artists: []
    };
	
	  this.search = this.search.bind(this);
  }
  
  search(e) {
    var term = document.getElementById("search_term").value;
    var _this = this;
    
    fetch("http://localhost:8080/artist/search?query=" + term).then(function(response){
        if (response.status !== 200) {  
          console.log('Looks like there was a problem. Status Code: ' +  response.status);  
          return;  
        }

        // Examine the text in the response  
        response.json().then(function(data) {  
          _this.setState(
            {
              artists: data
            });
        });  
      });  
  }
  
  render() {
    return (
		<div>
			<h3>Input artist name for search:</h3>
			<input type="text" id="search_term"/>
			<button type="button" className="btn-primary mx-sm-3" onClick={this.search}>Search</button>
			
      <ul className="list-group">
			{this.state.artists.map(function(artist, i){
			    return (
            <li className="list-group-item">
						  <Artist id={artist.id} thumbUrl={artist.thumbUrl} name={artist.name}/>
						</li>);
		  })}  
      </ul>  
		</div>  
    );
  }
}

export default Search;
