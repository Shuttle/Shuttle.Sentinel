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

resources.add('messagetypehandled', {action: 'list', permission: Permissions.Manage.Monitoring});

export const Map = DefineMap.extend({
    messageType: {
        type: 'string'
    },
    endpointCount: {
        type: 'number'
    }
});

var api = new Api({
    endpoint: 'messagetypeshandled/{search}',
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
                columnTitle: 'endpoint:message-type',
                columnClass: 'col',
                attributeName: "messageType"
            });

            columns.push({
                columnTitle: 'endpoint:endpoint-count',
                columnClass: 'col-2',
                attributeName: 'endpointCount'
            });
        }

        state.title = 'endpoint:message-types-handled';

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
    tag: 'sentinel-messagetypehandled-list',
    ViewModel,
    view
});