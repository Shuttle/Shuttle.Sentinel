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

$(function () {
	var localizationService = new Sentinel.Services.LocalizationService();
	var securityService = new Sentinel.Services.SecurityService();
	var services = {
		localizationService: localizationService,
		securityService: securityService
	};

	Sentinel.container.register('_localizationService', localizationService);
	Sentinel.container.register('_securityService', securityService);

	Sentinel.applicationState = new Sentinel.ApplicationState(services);

	securityService.fetchAnonymousPermissions()
		.done(function () {
			can.route(':resource');
			can.route(':resource/:action');

			can.route.map(Sentinel.applicationState.route);

			can.each(Sentinel.applicationStartCallbacks, function (callback) {
				callback(services);
			});

			localizationService.start(Sentinel.resources.localeNamespaces, function (err) {
				if (err) {
					throw new Error(err);
				}

				$('#application-container').html(can.view('#application-template', Sentinel.applicationState));

				can.route.ready();

				window.location.hash = securityService.hasPermission(Sentinel.Permissions.States.UserRequired)
					? '#!users/register'
					: '#!dashboard';
			});
		});
});
