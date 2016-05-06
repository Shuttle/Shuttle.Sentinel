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
Sentinel.ViewModel = can.Map.extend('ViewModel', {
}, {
    _localizedAttributes: [],

    init: function () {
        var self = this;

        this.resolve('_localizationService');

        this.bind('change', function (ev, attr, how, newVal, oldVal) {
        	if (attr.indexOf('_localizedAttributes') > -1) {
		        return;
        	}

	        var errors = self.errors(attr);

            if (errors && errors[attr].length > 0) {
                self.attr(attr + 'ValidationMessage', errors[attr][0]);
            } else {
                self.attr(attr + 'ValidationMessage', undefined);
            }

            if (!self._shouldLocalize(attr)) {
                return;
            }

            self._localizeAttributeValue(attr);
        });
    },

    _registerLocalizedAttribute: function (attr) {
        this._localizedAttributes.push(attr);

        this._localizeAttributeValue(attr);
    },

    _shouldLocalize: function (attr) {
        return $.inArray(attr, this._localizedAttributes) > -1;
    },

    _localizeAttributeValue: function (attr) {
    	var value = this.attr(attr);
    	var translation = this._localizationService.value(value);

    	if (translation !== value) {
		    this.attr(attr, translation);
	    }

        return translation;
    },

    hasErrors: function () {
    	return this.errors() != undefined;
    }
});

can.extend(Sentinel.ViewModel.prototype, Sentinel.Dependency);
