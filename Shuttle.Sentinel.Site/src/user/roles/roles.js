import Component from 'can-component/';
import DefineList from 'can-define/list/';
import DefineMap from 'can-define/map/';
import view from './roles.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import api from '~/api';
import $ from 'jquery';
import UserRole from '~/models/user-role';
import Role from '~/models/role';
import router from '~/router';

resources.add('user', { action: 'roles', permission: Permissions.Manage.Users });

export const ViewModel = DefineMap.extend(
    'user-role',
    {
        isResolved: { type: 'boolean', value: false },

        init: function() {
            var self = this;

            this.refresh();

            this.on('workingCount', function() {
                self.getRoleStatus();
            });
        },

        refresh() {
            const self = this;

            this.isResolved = false;

            self.roles.replace(new DefineList());

            Role.getList({})
                .then(function(availableRoles) {
                    availableRoles = $.makeArray(availableRoles);
                    availableRoles.push({ id: '', roleName: 'administrator' });

                    UserRole.getList({ id: router.data.id })
                        .then(function(userRoles) {
                            $.each(availableRoles,
                                function(availableRoleIndex, availableRole) {
                                    const active = userRoles.filter(function(item) {
                                        return item.roleName === availableRole.roleName;
                                    }).length > 0;
                                    const roleName = availableRole.roleName;

                                    self.roles.push(new UserRole({
                                        roleName: roleName,
                                        active: active
                                    }));
                                });
                        },
                        function(error) {
                            self.isResolved = true;
                        })
                        .then(function() {
                            self.isResolved = true;
                        });
                });
        },

        columns: {
            value: [
                {
                    columnTitle: 'active',
                    columnClass: 'col-md-1',
                    columnType: 'view',
                    view:
                        '<span ($click)="toggle()" class="glyphicon {{#if active}}glyphicon-check{{else}}glyphicon-unchecked{{/if}}" /><span class="glyphicon {{#if working}}glyphicon-hourglass{{/if}}" />'
                },
                {
                    columnTitle: 'user:roleName',
                    attributeName: 'roleName'
                }
            ]
        },

        roles: {
            value: new DefineList()
        },

        getRoleItem: function(roleName) {
            var result;

            $.each(this.roles,
                function(index, item) {
                    if (result) {
                        return;
                    }

                    if (item.roleName === roleName) {
                        result = item;
                    }
                });

            return result;
        },

        workingItems: {
            get() {
                return this.roles.filter(function(item) {
                    return item.working;
                });
            }
        },

        workingCount: {
            type: 'number',
            get() {
                return this.workingItems.length;
            }
        },

        getRoleStatus: function() {
            var self = this;

            if (this.workingCount === 0) {
                return;
            }

            var data = {
                userId: router.data.id,
                roles: []
            };

            $.each(this.workingItems,
                function(index, item) {
                    data.roles.push(item.roleName);
                });

            api.post('users/rolestatus', { data: data })
                .done(function(response) {
                    $.each(response.data,
                        function(index, item) {
                            const roleItem = self.getRoleItem(item.roleName);

                            if (!roleItem) {
                                return;
                            }

                            roleItem.working = !(roleItem.active === item.active);
                        });
                })
                .always(function() {
                    setTimeout(self.getRoleStatus(), 1000);
                });
        }
    });

export default Component.extend({
    tag: 'sentinel-user-roles',
    ViewModel,
    view
});