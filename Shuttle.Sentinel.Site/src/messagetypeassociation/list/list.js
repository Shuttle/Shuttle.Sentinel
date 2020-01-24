import {DefineMap,DefineList,Component} from 'can';
import view from './list.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import Api from 'shuttle-can-api';
import state from '~/state';

resources.add('messagetypeassociation', {action: 'list', permission: Permissions.Manage.Monitoring});

export const Map = DefineMap.extend({
    messageTypeHandled: {
        type: 'string'
    },
    messageTypeDispatched: {
        type: 'string'
    },
    endpointCount: {
        type: 'number'
    }
});

var api = new Api({
    endpoint: 'messagetypeassociations/{search}',
    Map
});

export const ViewModel = DefineMap.extend({
    columns: {
        Default: DefineList
    },

    refreshTimestamp: {type: 'string'},

    get listPromise() {
        const refreshTimestamp = this.refreshTimestamp;
        return api.list();
    },

    init: function () {
        const columns = this.columns;

        if (!columns.length) {
            columns.push({
                columnTitle: 'endpoint:message-type-handled',
                columnClass: 'col',
                attributeName: "messageTypeHandled"
            });

            columns.push({
                columnTitle: 'endpoint:message-type-dispatched',
                columnClass: 'col',
                attributeName: "messageTypeDispatched"
            });

            columns.push({
                columnTitle: 'endpoint:endpoint-count',
                columnClass: 'col-2',
                attributeName: 'endpointCount'
            });
        }

        state.title = 'endpoint:message-type-associations';

        state.navbar.addButton({
            type: 'refresh',
            viewModel: this
        });
    },

    refresh: function () {
        this.refreshTimestamp = Date.now();
    }
});


export default Component.extend({
    tag: 'sentinel-messagetypeassociation-list',
    ViewModel,
    view
});