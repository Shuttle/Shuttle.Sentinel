import Component from 'can-component/';
import DefineList from 'can-define/list/';
import DefineMap from 'can-define/map/';
import view from './permissions.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import api from '~/api';
import each from 'can-util/js/each/';
import router from '~/router';
import alerts from '~/alerts';
import localisation from '~/localisation';
import $ from 'jquery';

resources.add('role', { action: 'permissions', permission: Permissions.Manage.RolePermissions });

let PermissionModel = DefineMap.extend(
    'permission',
    {
        permission: {
            value: ''
        },

        active: {
            value: false
        },

        rowClass: {
            get: function() {
                return this.active ? 'text-success success' : 'text-muted';
            }
        },

        toggle: function() {
            if (this.working) {
                alerts.show({ message: localisation.value('workingMessage'), name: 'working-message' });
                return;
            }

            this.active = !this.active;
            this.working = true;

            api.post('roles/setpermission', { data: {
                roleId: router.data.id, 
                permission: this.permission,
                active: this.active
            } });

            this.viewModel._working();
        }
    }
    );

export const ViewModel = DefineMap.extend(
    'role-permission',
    {
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
            value: new DefineList()
        },

        init: function() {
            this.refresh();
        },

        add: function() {
        },

        refresh: function() {
            var self = this;

            self.permissions.replace(new DefineList());

            this.get('permissions')
                .done(function(availablePermissions) {
                    self.get('permissions/' + router.data.id)
                        .done(function(rolePermissions) {
                            each(availablePermissions, function(availablePermission) {
                                self.permissions.push(new PermissionModel({
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

            each(this.permissions, function(item) {
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
            var workingList = this.permissions.filter(function(item) {
                return item.working;
            });

            if (!workingList.length) {
                return;
            }

            var data = {
                roleId: router.data.id,
                permissions: []
            }

            each(workingList, function(item) {
                data.permissions.push(item.permission);
            });

            api.post('roles/permissionstatus', { data: data})
                .done(function(response) {
                    each(response.data, function(item) {
                        var permissionItem = self.getPermissionItem(item.permission);

                        if (!permissionItem) {
                            return;
                        }

                        permissionItem.working = !(permissionItem.active === item.active);
                    });
                })
                .always(function() {
                    setTimeout(self._working(), 1000);
                });
        }
    }
    );

export default Component.extend({
    tag: 'sentinel-role-permissions',
    ViewModel,
    view
});