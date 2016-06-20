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

			if (resource.name === name && resource.action === o.action) {
				result = resource;
			}
		});

		return result;
	}
});

Sentinel.resources = new Sentinel.Resources();
