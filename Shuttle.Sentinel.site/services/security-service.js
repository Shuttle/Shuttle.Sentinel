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