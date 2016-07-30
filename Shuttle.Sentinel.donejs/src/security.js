import can from 'can';
import localisation from 'sentinel/localisation';
import RegisterSession from 'sentinel/models/register-session';
import state from 'sentinel/application-state';
import alerts from 'sentinel/alerts';
import api from 'sentinel/api';

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

    fetchAnonymousPermissions: function () {
        var self = this;

        return api.get('anonymouspermissions')
            .done(function(data) {
                state.attr('requiresInitialAdministrator', data.requiresInitialAdministrator);

                can.each(data.permissions, function(permission) {
                    self._addPermission('anonymous', permission);
                });
            })
            .fail(function() {
                alerts.show({ message: localisation.value('exceptions.anonymous-permissions'), type: 'danger' });
            });
    },

    _addPermission: function (type, permission) {
        state.attr('permissions').push({ type: type, permission: permission });
    },

    login: function (username, password) {
        var self = this;
        var deferred = can.Deferred();

        new RegisterSession({
            username: username,
            password: password
        }).save()
			.done(function (response) {
			    if (response.registered) {
			        state.userLoggedIn(username, response.token);
			        alerts.remove({ name: 'login-failure' });

			        self.removeUserPermissions();

			        can.each(response.permissions, function (permission) {
			            self._addPermission('user', permission);
			        });

			        deferred.resolve();
			    } else {
			        alerts.show({ message: localisation.value('exceptions.login', { username: username }), type: 'danger', name: 'login-failure' });
			        deferred.reject();
			    }
			})
			.fail(function (error) {
			    alerts.show(error, 'danger');
			    deferred.reject();
			});

        return deferred;
    },

    logout: function() {
        state.attr('username', undefined);
        state.attr('token', undefined);

        this.removeUserPermissions();
    },

    removeUserPermissions: function() {
        state.attr('permissions', state.attr('permissions').filter(function(item) {
            return item.type !== 'user';
        }));
    }
};

export default security;