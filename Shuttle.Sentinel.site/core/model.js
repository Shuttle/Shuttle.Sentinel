Sentinel.Model = can.Model.extend({
	getApiUrl: function(controller) {
		return Sentinel.configuration.getApiUrl(controller);
	}
}, {});

can.extend(Sentinel.Model.prototype, Sentinel.Dependency);
