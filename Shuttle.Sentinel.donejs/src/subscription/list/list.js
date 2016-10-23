import Component from 'can/component/';
import List from 'can/list/';
import template from './list.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import Model from 'sentinel/model';
import alerts from 'sentinel/alerts';
import localisation from 'sentinel/localisation';

resources.add('subscription', { action: 'list', permission: Permissions.Manage.subscriptions });

export const ViewModel = Model.extend({
    define: {
        columns: {
            value: new List()
        }
    },

    init: function() {
        const columns = this.attr('columns');
        this.refresh();

        if (!columns.length) {
            columns.push({
                columnTitle: 'clone',
                columnClass: 'col-md-1',
                columnType: 'button',
                buttonTitle: 'clone',
                buttonClick: 'clone',
                buttonContext: this
            });

            columns.push({
                columnTitle: 'name',
                columnClass: 'col-md-1',
                attributeName: 'name'
            });

            columns.push({
                columnTitle: 'subscription:connection-string',
                attributeName: 'connectionString'
            });

            columns.push({
                columnTitle: 'subscription:provider-name',
                columnClass: 'col-md-1',
                attributeName: 'providerName'
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
        state.goto('subscription/add');
    },

    refresh: function() {
        this.get('subscriptions');
    },

    rowClick: function(row) {
        state.set('subscription', row);

        state.goto('subscription/edit');
    },

    remove: function(row) {
        this.post('subscriptions/remove', {
                name: row.attr('name')
            })
            .done(function() {
                alerts.show({ message: localisation.value('itemRemovalRequested', { itemName: localisation.value('subscription:title') }), name: 'item-removal' });
            });
    },

    clone: function(row) {
        state.set('subscription', row);

        this.add();
    }
});


export default Component.extend({
    tag: 'sentinel-subscription-list',
    viewModel: ViewModel,
    template
});