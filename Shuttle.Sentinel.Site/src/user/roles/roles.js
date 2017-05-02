import Component from 'can-component/';
import DefineList from 'can-define/list/';
import DefineMap from 'can-define/map/';
import view from './roles.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import state from '~/state';
import api from '~/api';
import $ from 'jquery';
import UserRoles from '~/models/user-roles';
import Roles from '~/models/user-roles';
import router from '~/router';

resources.add('user', { action: 'roles', permission: Permissions.Manage.Users });

export const ViewModel = DefineMap.extend(
    'user-roles',
    {
        isResolved: { type: 'boolean', value: false },

        refresh() {
            const self = this;

            this.isResolved = false;

            return Roles.getList({})
                .then(function(availableRoles) {
                    availableRoles = $.makeArray(availableRoles);
                    availableRoles.push({ id: '', roleName: 'administrator' });

                    UserRoles.getList({ id: router.data.id })
                        .then(function(userRoles) {
                            $.each(availableRoles,
                                function(availableRoleIndex, availableRole) {
                                    self.roles.push(new UserRoles({
                                        viewModel: self,
                                        roleName: availableRole.roleName,
                                        active: $.inArray(availableRole.roleName, userRoles) > -1
                                    }));
                                });
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

        init: function() {
            this.refresh();
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

        _working: function() {
            var self = this;
            const workingList = this.roles.filter(function(item) {
                return item.working;
            });

            if (!workingList.length) {
                return;
            }

            var data = {
                userId: state.attr('route.id'),
                roles: []
            };
            $.each(workingList,
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
    ViewModel,
    view
});