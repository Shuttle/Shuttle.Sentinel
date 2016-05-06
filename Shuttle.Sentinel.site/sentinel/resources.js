/*
    This file forms part of Shuttle.Sentinel.

    Shuttle.Sentinel - A management and monitoring solution for shuttle-esb implementations. 
    Copyright (C) 2015  Eben Roux

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
Sentinel.Resources = can.Map.extend({
	localeNamespaces: ['sentinel'],
	_resources: [],

	addLocaleNamespace: function (namespace) {
		this.localeNamespaces.push(namespace);
	},

	addResource: function (name, options) {
		var o = options || {};

		if (this.hasResource(name, options)) {
			throw new Error('Resource \'' + name + '\' has already been registered.');
		}

		this._resources.push({
			name: name,
			action: o.action,
			componentName: o.componentName,
			url: o.url,
			permission: o.permission
		});
	},

	hasResource: function (resource, options) {
		return this.findResource(name, options) != undefined;
	},

	findResource: function (name, options) {
		var o = options || {};
		var result = undefined;

		can.each(this._resources, function (resource) {
			if (result) {
				return;
			}

			if (resource.name === name && resource.action == o.action) {
				result = resource;
			}
		});

		return result;
	}
});

Sentinel.resources = new Sentinel.Resources();
