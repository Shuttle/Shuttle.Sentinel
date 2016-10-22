import Component from 'can/component/';
import List from 'can/list/';
import template from './list.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import Model from 'sentinel/model';
import alerts from 'sentinel/alerts';
import localisation from 'sentinel/localisation';

resources.add('datastore', { action: 'list', permission: Permissions.Manage.DataStores });

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
                columnTitle: 'datastore:connection-string',
                attributeName: 'connectionString'
            });

            columns.push({
                columnTitle: 'datastore:provider-name',
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
        state.goto('datastore/add');
    },

    refresh: function() {
        this.get('datastores');
    },

    rowClick: function() {
        state.goto('datastore/edit');
    },

    remove: function(row) {
        this.post('datastores/remove', {
                    uri: row.attr('uri')
                }
            )
            .done(function() {
                alerts.show({ message: localisation.value('itemRemovalRequested', { itemName: localisation.value('datastore:title') }), name: 'item-removal' });
            });
    },

    clone: function(row) {
        state.set('datastore', row);

        this.add();
    }
});


export default Component.extend({
    tag: 'sentinel-datastore-list',
    viewModel: ViewModel,
    template
});