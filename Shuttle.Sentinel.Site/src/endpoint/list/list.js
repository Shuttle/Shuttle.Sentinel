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

resources.add('endpoint', { action: 'list', permission: Permissions.Manage.Monitoring });

export const Map = DefineMap.extend({
    id: {
        type: 'string'
    },
    machineName: {
        type: 'string'
    },
    baseDirectory: {
        type: 'string'
    },
    ipv4Address: {
        type: 'string'
    },
    remove() {
        api.delete({id: this.id})
            .then(function () {
                state.alerts.show({
                    message: localisation.value('itemRemovalRequested',
                        {itemName: localisation.value('endpoint:title')})
                });
            });
    }
});

var api = new Api({
    endpoint: 'endpoints/{id}',
    Map
});

export const ViewModel = DefineMap.extend({
    columns: {
        Default: DefineList
    },

    refreshTimestamp: { type: 'string' },

    get listPromise() {
        const refreshTimestamp = this.refreshTimestamp;
        return api.list();
    },

    init: function() {
        const columns = this.columns;

        if (!columns.length) {
            columns.push({
                columnTitle: 'endpoint:machine-name',
                columnClass: 'col-2',
                attributeName: 'machineName'
            });

            columns.push({
                columnTitle: 'endpoint:base-directory',
                columnClass: 'col',
                attributeName: 'baseDirectory'
            });

            columns.push({
                columnTitle: 'endpoint:ipv4-address',
                columnClass: 'col-1',
                attributeName: 'ipv4Address'
            });

            columns.push({
                columnTitle: 'remove',
                columnClass: 'col-1',
                stache: '<cs-button-remove click:from="remove" elementClass:from="\'btn-sm\'"/>'
            });
        }

        state.title = 'endpoint:list.title';

        state.navbar.addButton({
            type: 'refresh',
            viewModel: this
        });
    },

    refresh: function() {
        this.refreshTimestamp = Date.now();
    }
});


export default Component.extend({
    tag: 'sentinel-endpoint-list',
    ViewModel,
    view
});