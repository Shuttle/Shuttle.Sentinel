import Component from 'can/component/';
import List from 'can/list/';
import Map from 'can/map/';
import template from './roles.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import api from 'sentinel/api';
import Model from 'sentinel/model';
import alerts from 'sentinel/alerts';
import localisation from 'sentinel/localisation';
import $ from 'jquery';

resources.add('user', { action: 'roles', permission: Permissions.Manage.Users });

let RoleModel = Map.extend({
    define: {
        roleName: {
            value: ''
        },

        active: {
            value: false
        },

        rowClass: {
            get: function() {
                return this.attr('active') ? 'text-success success' : 'text-muted';
            }
        }
    },

    toggle: function() {
        var self = this;

        if (this.attr('working')) {
            alerts.show({ message: localisation.value('workingMessage'), name: 'working-message' });
            return;
        }

        this.attr('active', !this.attr('active'));
        this.attr('working', true);

        api.post('users/setrole', { data: {
            userId: state.attr('route.id'), 
            roleName: this.attr('roleName'),
            active: this.attr('active')
        } })
        .done(function(response) {
            if (response.success) {
                return;
            }

            switch (response.failureReason.toLowerCase()) {
                case 'lastadministrator':
                    {
                        self.attr('active', true);
                        self.attr('working', false);

                        alerts.show({ message: localisation.value('user:exceptions.last-administrator'), name: 'last-administrator', type: 'danger' });
                        break;
                    }
            }
        });

        this.attr('viewModel')._working();
    }
});

export const ViewModel = Model.extend({
    define: {
        columns: {
            value: [
                {
                    columnTitle: 'active',
                    columnClass: 'col-md-1',
                    columnType: 'template',
                    template: '<span ($click)="toggle()" class="glyphicon {{#if active}}glyphicon-check{{else}}glyphicon-unchecked{{/if}}" /><span class="glyphicon {{#if working}}glyphicon-hourglass{{/if}}" />'
                },
                {
                    columnTitle: 'user:roleName',
                    attributeName: 'roleName'
                }
            ]
        },

        roles: {
            value: new List()
        }
    },

    init: function() {
        this.refresh();
    },

    add: function() {
    },

    refresh: function() {
        var self = this;

        self.attr('roles').replace(new List());

        this.get('roles')
            .done(function(availableRoles) {
                availableRoles = $.makeArray(availableRoles);
                availableRoles.push({ id: '', roleName: 'administrator' });

                self.get('users/' + state.attr('route.id') + '/roles')
                    .done(function(userRoles) {
                        $.each(availableRoles, function(availableRoleIndex, availableRole) {
                            self.attr('roles').push(new RoleModel({
                                viewModel: self,
                                roleName: availableRole.roleName,
                                active: $.inArray(availableRole.roleName, userRoles) > -1
                            }));
                        });
                    });
            });
    },

    getRoleItem: function(roleName) {
        var result;

        $.each(this.attr('roles'), function(index, item) {
            if (result) {
                return;
            }

            if (item.roleName === roleName) {
                result = item;
            }
        });

        return result;
    },

    _working: function() {
        var self = this;
        var workingList = this.attr('roles').filter(function(item) {
            return item.attr('working');
        });

        if (!workingList.length) {
            return;
        }

        var data = {
            userId: state.attr('route.id'),
            roles: []
        }

        $.each(workingList, function(index, item) {
            data.roles.push(item.attr('roleName'));
        });

        api.post('users/rolestatus', { data: data})
            .done(function(response) {
                $.each(response.data, function(index, item) {
                    var roleItem = self.getRoleItem(item.roleName);

                    if (!roleItem) {
                        return;
                    }

                    roleItem.attr('working', !(roleItem.active === item.active));
                });
            })
            .always(function() {
                setTimeout(self._working(), 1000);
            });
    }
});

export default Component.extend({
    tag: 'sentinel-user-roles',
    viewModel: ViewModel,
    template
});