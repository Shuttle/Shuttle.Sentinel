Sentinel.Control = can.Control.extend({
	init: function (localeTitleKey) {
		Sentinel.guard.againstUndefined(localeTitleKey, 'localeTitleKey');

		this.resolve('_securityService');
		this.resolve('_localizationService');

		this._title = this._localizationService.value(localeTitleKey);
	},

	requirePermission: function(permission) {
		if (this._securityService.hasPermission(permission)) {
			return;
		}

		Sentinel.applicationState.accessDenied(this._title, permission);
	}
});

can.extend(Sentinel.Control.prototype, Sentinel.Dependency);
