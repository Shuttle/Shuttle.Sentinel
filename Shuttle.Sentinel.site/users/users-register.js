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
Sentinel.Components.ViewModels.UserRegister = Sentinel.ViewModel.extend({
	init: function () {
		this.validatePresenceOf('email');
		this.validateFormatOf(['email'], /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i, {
			message: 'invalid email'
		});
		this.validatePresenceOf('password');
	}
}, {
	register: function() {
		alert('odd!');
	},

	submit: function () {
		alert(this.attr('email'));

		if (this.hasErrors()) {
			alert('errors');

			return;
		}

		alert('submit');
	}
});

Sentinel.Components.UserRegister = can.Component.extend({
	tag: 'sentinel-users-register',
	template: can.view('users/users-register.stache'),
	viewModel: Sentinel.Components.ViewModels.UserRegister
});

Sentinel.resources.addResource('users', { action: 'register' });
