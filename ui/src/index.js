import React from 'react';
import ReactDOM from 'react-dom';
import { Router, Route, hashHistory, IndexRoute  } from 'react-router';
import App from './components/App';
import About from './components/About';
import Search from './components/Search';
import ArtistDetails from './components/ArtistDetails';
import AlbumDetails from './components/AlbumDetails';

ReactDOM.render((
  <Router history={hashHistory}>
	<Route path="/" component={App}>
		<Route path="about" component={About}/>
		<Route path="search" component={Search}/>
		<Route path="artist/:id" component={ArtistDetails}/>
		<Route path="album/:id" component={AlbumDetails}/>
	</Route>
  </Router>
  ), document.getElementById('root')
);
