import Component from 'can/component/';
import 'can/map/define/';
import template from './list.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import Model from 'sentinel/model';
import alerts from 'sentinel/alerts';
import localisation from 'sentinel/localisation';

resources.add('user', { action: 'list', permission: Permissions.View.Users });

export const ViewModel = Model.extend({
    define: {
        columns: {
            value: [
                {
                    columnTitle: 'user:list.roles',
                    columnClass: 'col-md-1',
                    columnType: 'button',
                    buttonTitle: 'user:list.roles',
                    buttonClick: 'roles(id)'
                },
                {
                    columnTitle: 'user:username', 
                    attributeName: 'username'
                },
                {
                    columnTitle: 'user:dateRegistered', 
                    attributeName: 'dateRegistered'
                },
                {
                    columnTitle: 'user:registeredBy', 
                    attributeName: 'registeredBy'
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
        state.goto('user/register');
    },

    refresh: function() {
        this.get('users');
    },

    remove: function(id) {
        this.delete(`roles/${id}`)
            .done(function() {
                alerts.show({ message: localisation.value('itemRemovalRequested', { itemName: localisation.value('role:role') }) });
            });
    },

    roles: function(id) {
        state.goto('user/' + id + '/roles');
    }
});

export default Component.extend({
    tag: 'sentinel-user-list',
    viewModel: ViewModel,
    template
});