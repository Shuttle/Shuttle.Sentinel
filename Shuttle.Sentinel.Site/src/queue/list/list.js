import {DefineMap,DefineList,Component} from 'can';
import view from './list.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import router from '~/router';
import Api from 'shuttle-can-api';
import localisation from '~/localisation';
import state from '~/state';

resources.add('queue', { action: 'list', permission: Permissions.Manage.Queues });

export const Map = DefineMap.extend({
    id: {
        type: 'string'
    },
    uri: {
        type: 'string'
    },
    processor: {
        type: 'string'
    },
    type: {
        type: 'string'
    },
    securedUri: {
        type: 'string'
    },
    remove() {
        api.delete({id: this.id})
            .then(function () {
                state.alerts.add({
                    message: localisation.value('itemRemovalRequested',
                        {itemName: localisation.value('queue:title')})
                });
            });
    },
    clone() {
        state.stack.put('queue', this);

        router.goto({
            resource: 'queue',
            action: 'add'
        });
    }
});

var api = new Api({
    endpoint: 'queues/{id}',
    Map
});

export const ViewModel = DefineMap.extend({
    columns: {
        Default: DefineList
    },

    refreshTimestamp: { type: 'string' },

    get queuesPromise() {
        const refreshTimestamp = this.refreshTimestamp;
        return api.list();
    },

    init: function() {
        const columns = this.columns;

        if (!columns.length) {
            columns.push({
                columnTitle: 'clone',
                columnClass: 'col-1',
                stache: '<cs-button text:from="\'clone\'" click:from="clone" elementClass:from="\'btn-sm\'"/>'
            });

            columns.push({
                columnTitle: 'queue:queue-uri',
                columnClass: 'col',
                attributeName: 'uri'
            });

            columns.push({
                columnTitle: 'queue:processor',
                columnClass: 'col-1',
                attributeName: 'processor'
            });

            columns.push({
                columnTitle: 'queue:type',
                columnClass: 'col-1',
                attributeName: 'type'
            });

            columns.push({
                columnTitle: 'remove',
                columnClass: 'col-1',
                stache: '<cs-button-remove click:from="remove" elementClass:from="\'btn-sm\'"/>'
            });
        }

        state.title = 'queue:list.title';

        state.navbar.addButton({
            type: 'add',
            viewModel: this,
            permission: 'sentinel://queue/add'
        });

        state.navbar.addButton({
            type: 'refresh',
            viewModel: this
        });
    },

    add: function() {
        router.goto({
            resource: 'queue',
            action: 'add'
        });
    },

    refresh: function() {
        this.refreshTimestamp = Date.now();
    }
});


export default Component.extend({
    tag: 'sentinel-queue-list',
    ViewModel,
    view
});