import can from 'can';
import Map from 'can/map/';

var resources = new Map({
	_resources: [],

	add: function (name, options) {
		var o = options || {};

		if (this.has(name, options)) {
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

	has: function (resource, options) {
		return this.find(name, options) != undefined;
	},

	find: function (name, options) {
		var o = options || {};
		var result = undefined;

		can.each(this._resources, function (resource) {
			if (result) {
				return;
			}

			if (resource.name === name && resource.action === o.action) {
				result = resource;
			}
		});

		return result;
	}
});

export default resources;
