import can from 'can';
import Map from 'can/map/';
import localisation from 'sentinel/localisation';
import AnonymousPermissions from 'sentinel/models/anonymous-permissions';
import RegisterSession from 'sentinel/models/register-session';

var security = {
	_data: new Map({
		permissions: [],
		token: undefined
	}),

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

		AnonymousPermissions.findAll()
			.done(function (data) {
				can.each(data, function (item) {
					self._addPermission('anonymous', item.permission);
				});

				deferred.resolve();
			})
			.fail(function () {
				deferred.reject(localisation.value('exceptions.anonymous-permissions'));
			});

		return deferred;
	},

	_addPermission: function (type, permission) {
		this._data.attr('permissions').push({ type: type, permission: permission });
	},

	login: function (email, password) {
		new RegisterSession({
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
			window.location.hash = '#!login';
		}
	}
};

export default security;