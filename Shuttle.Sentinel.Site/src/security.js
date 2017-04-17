import $ from 'jquery';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import api from '~/api';
import localisation from '~/localisation';
import alerts from '~/alerts';
import each from 'can-util/js/each/';
//import state from 'sentinel/state';

var Security = DefineMap.extend({
    isUserRequired: 'boolean',
    permissions: DefineList,

    hasSession: function() {
        return this.token != undefined;
    },

    hasPermission: function(permission) {
        var result = false;
        var permissionCompare = permission.toLowerCase();

        this.permissions.each(function(item) {
            if (result) {
                return;
            }

            result = item.permission === '*' || item.permission.toLowerCase() === permissionCompare;
        });

        return result;
    },

    removePermission: function(permission) {
        this.permissions = this.permissions.filter(function(item) {
            return item.permission !== permission;
        });
    },

    start: function() {
        var self = this;
        var deferred = $.Deferred();

        api.get('anonymouspermissions')
            .done(function(data) {
                const username = localStorage.getItem('username');
                const token = localStorage.getItem('token');
                this.isUserRequired = data.isUserRequired;

                each(data.permissions, function(item) {
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

    _addPermission: function(type, permission) {
        this.permissions.push({ type: type, permission: permission });
    },

    login: function(options) {
        var self = this;

        if (!options) {
            return $.Deferred().reject();
        }

        var usingToken = !!options.token;

        return api.post('sessions', {
            data: {
                username: options.username,
                password: options.password,
                token: options.token
            }
        })
            .done(function(response) {
                if (response.registered) {
                    localStorage.setItem('username', options.username);
                    localStorage.setItem('token', response.token);

                    alerts.remove({ name: 'login-failure' });

                    self.removeUserPermissions();

                    each(response.permissions, function(permission) {
                        self._addPermission('user', permission);
                    });
                } else {
                    if (usingToken) {
                        alerts.show({ message: localisation.value('exceptions.login', { username: options.username }), type: 'danger', name: 'login-failure' });
                    }
                }
            })
            .fail(function(error) {
                alerts.show(error, 'danger');
            });
    },

    logout: function() {
        this.username = undefined;
        this.token = undefined;

        localStorage.removeItem('username');
        localStorage.removeItem('token');

        this.removeUserPermissions();
    },

    removeUserPermissions: function() {
        this.permissions = this.permissions.filter(function(item) {
            return item.type !== 'user';
        });
    }
});

var security = new Security();

$.ajaxPrefilter(function(options, originalOptions) {
    options.beforeSend = function(xhr) {
        if (security.token) {
            xhr.setRequestHeader('sentinel-sessiontoken', state.attr('token'));
        }

        if (originalOptions.beforeSend) {
            originalOptions.beforeSend(xhr);
        }
    };
});

export default security;