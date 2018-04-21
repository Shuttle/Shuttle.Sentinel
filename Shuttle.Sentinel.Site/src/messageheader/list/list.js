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
    },
    remove() {
        api.delete({id: this.id})
            .then(function () {
                state.alerts.show({
                    message: localisation.value('itemRemovalRequested',
                        {itemName: localisation.value('messageheader:item.title')})
                });
            });
    },
    edit() {
        state.stack.put('messageheader', this);

        router.goto({
            resource: 'messageheader',
            action: 'item'
        });
    }
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
                columnTitle: 'edit',
                columnClass: 'col-1',
                stache: '<cs-button text:from="\'edit\'" click:from="edit" elementClass:from="\'btn-sm\'"/>'
            });

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
                columnTitle: 'remove',
                columnClass: 'col-1',
                stache: '<cs-button-remove click:from="remove" elementClass:from="\'btn-sm\'"/>'
            });
        }

        state.title = 'messageheader:list.title';

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
            action: 'item'
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