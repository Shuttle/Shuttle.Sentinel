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
Sentinel.Components.ViewModels.Login = Sentinel.ViewModel.extend({
	init: function () {
        this.validatePresenceOf('email');
        this.validatePresenceOf('password');
    }
}, {
    email: '',
    password: '',
    isEMailDisabled: false,
    isPasswordDisabled: false,

	init: function() {
		this.resolve('_securityService');
	},

    login: function() {
		this._securityService.login(this.attr('email'), this.attr('password'));
	}
});

Sentinel.Components.Login = can.Component.extend({
	tag: 'sentinel-login',
	template: can.view('login/login.stache'),
	viewModel: Sentinel.Components.ViewModels.Login
});

Sentinel.resources.addResource('login', { titleLocaleKey: 'credentials.login' });
