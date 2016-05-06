/*
    This file forms part of Shuttle.Sentinel.

    Shuttle.Sentinel - A management and monitoring solution for shuttle-esb implementations. 
    Copyright (C) 2016  Eben Roux

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
Sentinel.Services.CacheService = Sentinel.Service.extend({
	getItem: function(key) {
		var result = localStorage.getItem(key);

		return (result)
			? JSON.parse(result)
			: {};
	},

	setItem: function(key, data) {
		Sentinel.guard.againstUndefined(key, 'key');
		Sentinel.guard.againstUndefined(data, 'data');

		this._adapter.setItem(key, JSON.stringify(data));
	},

	removeItem: function(key) {
		this._adapter.removeItem(key);
	}
});