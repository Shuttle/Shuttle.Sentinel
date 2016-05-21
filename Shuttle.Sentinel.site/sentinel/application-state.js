Sentinel.ApplicationState = can.Map.extend({
	route: new can.Map(),

	init: function (services) {
		var self = this;
		var callContext = function (ev, prop, change, newVal, oldVal) {
			self.handleRoute.call(self, ev, prop, change, newVal, oldVal);
		};

		Sentinel.guard.againstUndefined(services, 'services');
		Sentinel.guard.againstUndefined(services.localizationService, 'services.localizationService');
		Sentinel.guard.againstUndefined(services.securityService, 'services.securityService');

		this._securityService = services.securityService;
		this._localizationService = services.localizationService;

		this.route.bind('change', callContext);
	},

	handleRoute: function (ev, prop, change, newVal, oldVal) {
		var resource;
		var componentName;
		var resourceName = this.route.attr('resource');
		var actionName = this.route.attr('action');
		var isActionRoute = ev.target.route === ':resource/:action';

		if (!resourceName) {
			return;
		}
		if (isActionRoute) {
			if (prop === 'resource') {
				return;
			}

			resource = Sentinel.resources.findResource(resourceName, { action: actionName });
		} else {
			resource = Sentinel.resources.findResource(resourceName);
		}

		if (!resource) {
			Sentinel.logger.error(this._localizationService.value('exceptions.resource-not-found', { hash: window.location.hash }));

			window.location.hash = '#!dashboard';

			return;
		}

		if (resource.permission && !this._securityService.hasPermission(resource.permission)) {
			this._securityService.accessDenied(resource.permission);
		}

		componentName = resource.componentName || 'sentinel-' + resource.name + (isActionRoute ? '-' + actionName : '');

		$('#application-content').html(can.stache('<' + componentName + '></' + componentName + '>')(this));
	}
});
