import Component from 'can/component/';
import 'can/map/define/';
import template from './list.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import Model from 'sentinel/model';
import alerts from 'sentinel/alerts';
import localisation from 'sentinel/localisation';

resources.add('role', { action: 'list', permission: Permissions.View.Roles });

export const ViewModel = Model.extend({
    define: {
        columns: {
            value: [
                {
                    columnTitle: 'role:permissions.title',
                    columnClass: 'col-md-1',
                    columnType: 'button',
                    buttonTitle: 'role:permissions.title',
                    buttonClick: 'permissions(id)'
                },
                {
                    columnTitle: 'role:name', 
                    attributeName: 'rolename'
                },
                {
                    columnTitle: 'remove', 
                    columnClass: 'col-md-1',
                    columnType: 'remove-button',
                    buttonTitle: 'role:permissions.title',
                    buttonClick: 'remove(id)'
                }
            ]
        }
    },

    init: function() {
        this.refresh();
    },

    add: function() {
        state.goto('role/add');
    },

    refresh: function() {
        this.get('roles');
    },

    remove: function(id) {
        this.delete(`roles/${id}`)
            .done(function() {
                alerts.show({ message: localisation.value('itemRemovalRequested', { itemName: localisation.value('role:role') }) });
            });
    },

    permissions: function(id) {
        state.goto(`role/${id}/permissions`);
    }
});


export default Component.extend({
    tag: 'sentinel-role-list',
    viewModel: ViewModel,
    template
});