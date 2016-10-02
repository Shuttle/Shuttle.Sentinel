import Component from 'can/component/';
import List from 'can/list/';
import Map from 'can/map/';
import template from './permissions.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import api from 'sentinel/api';
import Model from 'sentinel/model';
import alerts from 'sentinel/alerts';
import localisation from 'sentinel/localisation';

resources.add('role', { action: 'permissions', permission: Permissions.Manage.RolePermissions });

let PermissionModel = Map.extend({
    define: {
        permission: {
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
        if (this.attr('working')) {
            alerts.show({ message: localisation.value('workingMessage'), name: 'working-message' });
            return;
        }

        this.attr('active', !this.attr('active'));
        this.attr('working', true);

        api.post('roles/setpermission', { data: {
            roleId: state.attr('route.id'), 
            permission: this.attr('permission'),
            active: this.attr('active')
        } });

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
                    columnTitle: 'role:permission',
                    attributeName: 'permission'
                }
            ]
        },

        permissions: {
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

        self.attr('permissions').replace(new List());

        this.get('permissions')
            .done(function(availablePermissions) {
                self.get('permissions/' + state.attr('route.id'))
                    .done(function(rolePermissions) {
                        $.each(availablePermissions, function(availablePermissionIndex, availablePermission) {
                            self.attr('permissions').push(new PermissionModel({
                                viewModel: self,
                                permission: availablePermission,
                                active: $.inArray(availablePermission, rolePermissions) > -1
                            }));
                        });
                    });
            });
    },

    getPermissionItem: function(permission) {
        var result;

        $.each(this.attr('permissions'), function(index, item) {
            if (result) {
                return;
            }

            if (item.permission === permission) {
                result = item;
            }
        });

        return result;
    },

    _working: function() {
        var self = this;
        var workingList = this.attr('permissions').filter(function(item) {
            return item.attr('working');
        });

        if (!workingList.length) {
            return;
        }

        var data = {
            roleId: state.attr('route.id'),
            permissions: []
        }

        $.each(workingList, function(index, item) {
            data.permissions.push(item.attr('permission'));
        });

        api.post('roles/permissionstatus', { data: data})
            .done(function(response) {
                $.each(response.data, function(index, item) {
                    var permissionItem = self.getPermissionItem(item.permission);

                    if (!permissionItem) {
                        return;
                    }

                    permissionItem.attr('working', !(permissionItem.active === item.active));
                });
            })
            .always(function() {
                setTimeout(self._working(), 1000);
            });
    }
});

export default Component.extend({
    tag: 'sentinel-role-permissions',
    viewModel: ViewModel,
    template
});