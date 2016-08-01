import $ from 'jquery';
import can from 'can';
import localisation from 'sentinel/localisation';
import RegisterSession from 'sentinel/models/register-session';
import state from 'sentinel/state';
import alerts from 'sentinel/alerts';
import api from 'sentinel/api';

$.ajaxPrefilter(function( options, originalOptions, jqXHR ) {
    options.beforeSend = function (xhr) {
        if (state.attr('token')) {
            xhr.setRequestHeader('sentinel-sessiontoken', state.attr('token'));
        }

        if (originalOptions.beforeSend) {
            originalOptions.beforeSend(xhr);
        }
    }
});

var security = {
    hasSession: function () {
        return state.attr('token') != undefined;
    },

    hasPermission: function (permission) {
        var result = false;
        var permissionCompare = permission.toLowerCase();

        state.attr('permissions').each(function (item) {
            if (result) {
                return;
            }

            result = item.permission === '*' || item.permission.toLowerCase() === permissionCompare;
        });

        return result;
    },

    removePermission: function(permission) {
        state.attr('permissions', state.attr('permissions').filter(function(item) {
            return item.permission !== permission;
        }));
    },

    start: function () {
        var self = this;
        var deferred = $.Deferred();

        api.get('anonymouspermissions')
            .done(function(data) {
                var username = localStorage.getItem('username');
                var token = localStorage.getItem('token');

                state.attr('requiresInitialAdministrator', data.requiresInitialAdministrator);

                can.each(data.permissions, function(item) {
                    self._addPermission('anonymous', item.permission);
                });

                if (!!username && !!token) {
                    self.login({ username: username, token: token })
                        .done(function() {
                            deferred.resolve();
                        })
                        .fail(function() {
                            deferred.reject();
                        });
                } else {
                    deferred.resolve();
                }
            })
            .fail(function() {
                alerts.show({ message: localisation.value('exceptions.anonymous-permissions'), type: 'danger' });

                deferred.reject();
            });

        return deferred;
    },

    _addPermission: function (type, permission) {
        state.attr('permissions').push({ type: type, permission: permission });
    },

    login: function (options) {
        var self = this;

        if (!options) {
            return $.Deferred().reject();
        }

        var usingToken = !!options.token;

        return new RegisterSession({
            username: options.username,
            password: options.password,
            token: options.token
        }).save()
			.done(function (response) {
			    if (response.registered) {
			        state.userLoggedIn(options.username, response.token);

			        localStorage.setItem('username', options.username);
			        localStorage.setItem('token', response.token);
			        
			        alerts.remove({ name: 'login-failure' });

			        self.removeUserPermissions();

			        can.each(response.permissions, function (permission) {
			            self._addPermission('user', permission);
			        });
			    } else {
			        if (usingToken) {
			            alerts.show({ message: localisation.value('exceptions.login', { username: options.username }), type: 'danger', name: 'login-failure' });
			        }
			    }
			})
			.fail(function (error) {
			    alerts.show(error, 'danger');
			});
    },

    logout: function() {
        state.attr('username', undefined);
        state.attr('token', undefined);

        localStorage.removeItem('username');
        localStorage.removeItem('token');

        this.removeUserPermissions();
    },

    removeUserPermissions: function() {
        state.attr('permissions', state.attr('permissions').filter(function(item) {
            return item.type !== 'user';
        }));
    }
};

export default security;