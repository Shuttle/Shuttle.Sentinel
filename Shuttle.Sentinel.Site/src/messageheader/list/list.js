import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import resources from '~/resources';
import router from '~/router';
import localisation from '~/localisation';
import Permissions from '~/permissions';
import view from './list.stache!';
import Api from 'shuttle-can-api';
import state from '~/state';

resources.add('messageheader', {action: 'list', permission: Permissions.Manage.Messages});

export const Map = DefineMap.extend({
    key: {
        type: 'string',
        default: ''
    },
    value: {
        type: 'string',
        default: ''
    }
});

export const MessageHeaderList = DefineList.extend({
    '#': Map
});

var api = new Api({
    endpoint: 'messageheaders/{id}',
    Map: Map
});

export const ViewModel = DefineMap.extend({
    columns: {
        Default: DefineList
    },

    refreshTimestamp: {
        type: 'string'
    },

    get list() {
        const refreshTimestamp = this.refreshTimestamp;
        return api.list();
    },

    init() {
        const columns = this.columns;

        if (!columns.length) {
            columns.push({
                columnTitle: 'key',
                columnClass: 'col-5',
                attributeName: 'key'
            });

            columns.push({
                columnTitle: 'value',
                columnClass: 'col',
                attributeName: 'value'
            });

            columns.push({
                columnTitle: 'actions',
                columnClass: 'col-1',
                stache: '<cs-button click:from="scope.root.edit" text:from="\'edit\'" elementClass:from="\'btn-sm\'"/><cs-button-remove click:from="remove" elementClass:from="\'btn-sm\'"/>'
            });
        }

        state.title = localisation.value('messageheader:list.title');

        state.navbar.addButton({
            type: 'add',
            viewModel: this,
            permission: 'sentinel://messageheader/add'
        });

        state.navbar.addButton({
            type: 'refresh',
            viewModel: this
        });
    },

    add: function() {
        router.goto({
            resource: 'messageheader',
            action: 'add'
        });
    },

    refresh: function() {
        this.refreshTimestamp = Date.now();
    }
});

export default Component.extend({
    tag: 'sentinel-messageheader-list',
    ViewModel,
    view
});