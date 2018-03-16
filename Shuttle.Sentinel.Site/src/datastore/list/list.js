import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import view from './list.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import router from '~/router';
import Api from 'shuttle-can-api';
import localisation from '~/localisation';
import state from '~/state';

resources.add('datastore', { action: 'list', permission: Permissions.Manage.DataStores });

var datastores = new Api({ endpoint: 'datastores/{id}' });

export const ViewModel = DefineMap.extend({
    columns: {
        Default: DefineList
    },

    refreshTimestamp: {
        type: 'string'
    },

    get list () {
        const refreshTimestamp = this.refreshTimestamp;
        return datastores.list();
    },

    init() {
        const columns = this.columns;

        if (!columns.length) {
            columns.push({
                columnTitle: 'clone',
                columnClass: 'col-1',
                stache: '<cs-button text:from="\'clone\'" click:from="@clone" elementClass:from="\'btn-sm\'"/>'
            });

            columns.push({
                columnTitle: 'name',
                columnClass: 'col-2',
                attributeName: 'name'
            });

            columns.push({
                columnTitle: 'datastore:connection-string',
                columnClass: 'col',
                attributeName: 'connectionString'
            });

            columns.push({
                columnTitle: 'datastore:provider-name',
                columnClass: 'col-2',
                attributeName: 'providerName'
            });

            columns.push({
                columnTitle: 'remove',
                columnClass: 'col-1',
                stache: '<cs-button-remove click:from="@remove" elementClass:from="\'btn-sm\'"/>'
            });
        }

        state.title = localisation.value('datastore:list.title');

        state.navbar.addButton({
            type: 'add',
            viewModel: this,
            permission: 'sentinel://datastore/add'
        });

        state.navbar.addButton({
            type: 'refresh',
            viewModel: this
        });
    },

    add: function() {
        router.goto('datastore/add');
    },

    refresh: function() {
        this.refreshTimestamp = Date.now();
    },

    remove: function(row) {
        datastores.delete({ id: row.id })
            .then(function() {
                state.alerts.show({ message: localisation.value('itemRemovalRequested', { itemName: localisation.value('datastore:title') }), name: 'item-removal' });
            });
    },

    clone: function(row) {
        state.push('datastore', row);

        this.add();
    }
});

export default Component.extend({
    tag: 'sentinel-datastore-list',
    ViewModel,
    view
});