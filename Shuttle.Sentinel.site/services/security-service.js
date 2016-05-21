Sentinel.Services.SecurityService = Sentinel.Service.extend({
	_data: new can.Map({
		permissions: [],
		token: undefined
	}),

	init: function () {
		this.resolve('_localizationService');
		this.resolve('_cacheService');
	},

	hasSession: function () {
		return this._data.attr('token') != undefined;
	},

	hasPermission: function (permission) {
		var result = false;
		var permissionCompare = permission.toLowerCase();

		can.each(this._data.attr('permissions'), function (candidate) {
			if (result) {
				return;
			}

			result = candidate.permission === '*' || candidate.permission.toLowerCase() === permissionCompare;
		});

		return result;
	},

	fetchAnonymousPermissions: function () {
		var self = this;
		var deferred = can.Deferred();

		Sentinel.Models.AnonymousPermissions.findAll()
			.done(function (data) {
				can.each(data, function (item) {
					self._addPermission('anonymous', item.permission);
				});

				deferred.resolve();
			})
			.fail(function () {
				throw new Error(this._localizationService.value('exceptions.anonymous-permissions'));
			});

		return deferred;
	},

	_addPermission: function (type, permission) {
		this._data.attr('permissions').push({ type: type, permission: permission });
	},

	login: function (email, password) {
		new Sentinel.Models.RegisterSession({
			email: email,
			password: password
		}).save()
			.done(function (response) {
				alert('logged in');
			})
			.fail(function () {
				alert('login failure!');
			});
	},

	logout: function() {
		
	},

	accessDenied: function (permission) {
		alert(this._localizatioService.value('security.access-denied', { hash: window.location.hash, permission: permission }));

		if (!hasSession) {
			window.location.has = '#!login';
		}
	}
});