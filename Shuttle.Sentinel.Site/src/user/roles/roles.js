import Component from 'can-component/';
import DefineList from 'can-define/list/';
import DefineMap from 'can-define/map/';
import view from './roles.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import state from '~/state';
import api from '~/api';
import alerts from '~/alerts';
import localisation from '~/localisation';
import $ from 'jquery';
//import Roles from '../user-model';

resources.add('user', { action: 'roles', permission: Permissions.Manage.Users });

const UserRoles = DefineMap.extend({
    seal: false
}, {
    roleName: 'string',
    active: 'boolean',
    rowClass: {
        get: function() {
            return this.active ? 'text-success success' : 'text-muted';
        }
    },

    toggle: function() {
        var self = this;

        if (this.working) {
            alerts.show({ message: localisation.value('workingMessage'), name: 'working-message' });
            return;
        }

        this.attr('active', !this.active);
        this.working = true;

        api.post('users/setrole', { data: {
            userId: state.attr('route.id'), 
            roleName: this.roleName,
            active: this.active
        } })
        .done(function(response) {
            if (response.success) {
                return;
            }

            switch (response.failureReason.toLowerCase()) {
                case 'lastadministrator':
                    {
                        self.active = true;
                        self.working = false;

                        alerts.show({ message: localisation.value('user:exceptions.last-administrator'), name: 'last-administrator', type: 'danger' });

                        break;
                    }
            }
        });

        this.viewModel._working();
    }
});

const algebra = new set.Algebra(
  set.props.id('id')
);

UserRoles.List = DefineList.extend({
    '#': Model
});

UserRoles.connection = superMap({
    url: loader.serviceBaseURL + 'users/{id}/roles',
    Map: UserRoles,
    List: UserRoles.List,
    name: 'user',
    algebra
});

export const ViewModel = DefineMap.extend({
    get rolesPromise() {
        return UserRoles.getList({});
    },

    columns: {
        value: [
            {
                columnTitle: 'active',
                columnClass: 'col-md-1',
                columnType: 'view',
                view: '<span ($click)="toggle()" class="glyphicon {{#if active}}glyphicon-check{{else}}glyphicon-unchecked{{/if}}" /><span class="glyphicon {{#if working}}glyphicon-hourglass{{/if}}" />'
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

    refresh: function() {
        var self = this;

        self.roles.replace(new DefineList());

        this.get('roles')
            .done(function(availableRoles) {
                availableRoles = $.makeArray(availableRoles);
                availableRoles.push({ id: '', roleName: 'administrator' });

                self.get('users/' + state.attr('route.id') + '/roles')
                    .done(function(userRoles) {
                        $.each(availableRoles, function(availableRoleIndex, availableRole) {
                            self.roles.push(new UserRoles({
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

        $.each(this.roles, function(index, item) {
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
        var workingList = this.roles.filter(function(item) {
            return item.working;
        });

        if (!workingList.length) {
            return;
        }

        var data = {
            userId: state.attr('route.id'),
            roles: []
        }

        $.each(workingList, function(index, item) {
            data.roles.push(item.roleName);
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
    ViewModel,
    view
});