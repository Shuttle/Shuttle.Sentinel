import Component from 'can/component/';
import List from 'can/list/';
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
            value: new List()
        }
    },

    init: function() {
        let columns = this.attr('columns');

        this.refresh();

        if (!columns.length) {
            columns.push({
                columnTitle: 'user:list.roles',
                columnClass: 'col-md-1',
                columnType: 'button',
                buttonTitle: 'user:list.roles',
                buttonClick: 'roles',
                buttonContext: this
            });

            columns.push({
                columnTitle: 'user:username', 
                attributeName: 'username'
            });

            columns.push({
                columnTitle: 'user:dateRegistered', 
                attributeName: 'dateRegistered'
            });

            columns.push({
                columnTitle: 'user:registeredBy', 
                attributeName: 'registeredBy'
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
        state.goto('user/register');
    },

    refresh: function() {
        this.get('users');
    },

    remove: function(id) {
        this.delete(`roles/${id}`)
            .done(function() {
                alerts.show({ message: localisation.value('itemRemovalRequested', { itemName: localisation.value('user:title') }) });
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