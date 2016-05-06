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
