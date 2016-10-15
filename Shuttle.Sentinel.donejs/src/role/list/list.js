import Component from 'can/component/';
import template from './list.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import Model from 'sentinel/model';
import alerts from 'sentinel/alerts';
import localisation from 'sentinel/localisation';
import List from 'can/list/';

resources.add('role', { action: 'list', permission: Permissions.Manage.Roles });

export const ViewModel = Model.extend({
    define: {
        columns: {
            value: new List()
        }
    },

    init: function() {
        let columns = this.attr('columns');

        this.refresh();

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
        state.goto('role/add');
    },

    refresh: function() {
        this.get('roles');
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
    viewModel: ViewModel,
    template
});