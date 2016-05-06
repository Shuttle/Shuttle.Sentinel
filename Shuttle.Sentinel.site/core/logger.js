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
Sentinel.Logger = can.Construct.extend({
    _displayMessage: function (message, options) {
        options = options ? options : {};

        if ((options.type === 'error' || options.type === 'warn') || Sentinel.DEBUG) {
            var logMessage = '[' + new Date().toTimeString() + '] : ' + message;

            switch (options.type) {
                case 'debug':
                    console.debug(logMessage);
                    break;
                case 'warn':
                    console.warn(logMessage);
                    break;
                case 'error':
                    console.error(logMessage);
                    break;
                default:
                    console.log(logMessage);
                    break;
            }
        }
    },

    debug: function (message) {
        this._displayMessage(message, { type: 'debug' });
    },

    info: function (message) {
        this._displayMessage(message, { type: 'info' });
    },

    warn: function (message) {
        this._displayMessage(message, { type: 'warn' });
    },

    error: function (message) {
        this._displayMessage(message, { type: 'error' });
    }
});

Sentinel.logger = new Sentinel.Logger();
