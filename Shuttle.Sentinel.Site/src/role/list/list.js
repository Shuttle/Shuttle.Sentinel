import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import view from './list.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import router from '~/router';
import Role from '~/models/role';
import alerts from '~/alerts';
import localisation from '~/localisation';

resources.add('role', { action: 'list', permission: Permissions.Manage.Roles });

export const ViewModel = DefineMap.extend(
    'role-list',
    {
        refreshTimestamp: {
            type: 'string'
        },

        get rolesPromise() {
            const refreshTimestamp = this.refreshTimestamp;
            return Role.getList({});
        },

        columns: {
            Value: DefineList
        },

        init: function() {
            const columns = this.columns;

            if (!columns.length) {
                columns.push( {
                    columnTitle: 'role:permissions.title',
                    columnClass: 'col-md-1',
                    columnType: 'button',
                    buttonTitle: 'role:permissions.title',
                    buttonClick: 'permissions',
                    buttonContext: this
                });

                columns.push({
                    columnTitle: 'role:name', 
                    attributeName: 'roleName'
                });

                columns.push({
                    columnTitle: 'remove', 
                    columnClass: 'col-md-1',
                    columnType: 'remove-button',
                    buttonContext: this,
                    buttonClick: 'remove'
                });
            }
        },

        add: function() {
            router.goto('role/add');
        },

        refresh: function() {
            this.refreshTimestamp = Date.now();
        },

        remove: function(row) {
            this.delete('roles/' + row.attr('id'))
                .done(function() {
                    alerts.show({ message: localisation.value('itemRemovalRequested', { itemName: localisation.value('role:role') }) });
                });
        },

        permissions: function(row) {
            state.goto('role/' + row.attr('id') + '/permissions');
        }
    });


export default Component.extend({
    tag: 'sentinel-role-list',
    ViewModel,
    view
});