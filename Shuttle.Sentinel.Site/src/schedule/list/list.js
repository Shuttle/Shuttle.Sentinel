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
import each from 'can-util/js/each/';
import {OptionList} from 'shuttle-canstrap/select/';

resources.add('schedule', {action: 'list', permission: Permissions.Manage.Schedules});

const ScheduleMap = DefineMap.extend({
    dataStoreId: {
        type: 'string'
    },

    id: {
        type: 'string',
        default: ''
    },

    name: {
        type: 'string',
        default: ''
    },

    inboxWorkQueueUri: {
        type: 'string',
        default: ''
    },

    securedUri: {
        type: 'string',
        default: ''
    },

    cronExpression: {
        type: 'string',
        default: ''
    },

    nextNotificaton: {
        type: 'date',
        default: ''
    },

    remove() {
        api.schedules.delete({ dataStoreId: this.dataStoreId, id: this.id })
            .then(function () {
                state.alerts.show({
                    message: localisation.value('itemRemovalRequested', {itemName: localisation.value('schedule:title')}),
                    name: 'item-removal'
                });
            });
    },

    clone: function () {
        const serialized = this.serialize();

        serialized.dataStoreId = this.dataStoreId;

        state.stack.put('subscription', serialized);

        router.goto({
            resource: 'subscription',
            action: 'add'
        });
    },

    edit() {
        state.stack.put('schedule', this);

        router.goto({
            resource: 'schedule',
            action: 'item'
        });
    }
});

var api = {
    schedules: new Api({
        endpoint: 'schedules/{dataStoreId}/{id}',
        Map: ScheduleMap
    }),
    stores: new Api({
        endpoint: 'datastores'
    })
}


export const ViewModel = DefineMap.extend(
    'schedules',
    {
        columns: {
            Default: DefineList
        },

        refreshTimestamp: {
            type: 'string'
        },

        dataStoreId: {
            type: 'string',
            default: ''
        },

        dataStores: {
            Default: OptionList
        },

        get listPromise() {
            const refreshTimestamp = this.refreshTimestamp;
            const dataStoreId = this.dataStoreId;

            return !dataStoreId ? undefined : api.schedules.list({id: dataStoreId});
        },

        init() {
            const self = this;
            const columns = this.columns;

            if (!columns.length) {
                columns.push({
                    columnTitle: 'clone',
                    columnClass: 'col-1',
                    stache: '<cs-button text:from="\'clone\'" click:from="clone" elementClass:from="\'btn-sm\'"/>'
                });

                columns.push({
                    columnTitle: 'edit',
                    columnClass: 'col-1',
                    stache: '<cs-button text:from="\'edit\'" click:from="edit" elementClass:from="\'btn-sm\'"/>'
                });

                columns.push({
                    columnTitle: 'name',
                    columnClass: 'col-1',
                    attributeName: 'name'
                });

                columns.push({
                    columnTitle: 'schedule:inbox-work-queue-uri',
                    columnClass: 'col',
                    attributeName: 'securedUri'
                });

                columns.push({
                    columnTitle: 'schedule:cron-expression',
                    columnClass: 'col',
                    attributeName: 'cronExpression'
                });

                columns.push({
                    columnTitle: 'schedule:next-notification',
                    columnClass: 'col',
                    attributeName: 'nextNotification'
                });

                columns.push({
                    columnTitle: 'remove',
                    columnClass: 'col-1',
                    stache: '<cs-button-remove click:from="remove" elementClass:from="\'btn-sm\'"/>'
                });
            }

            self.dataStores.push({
                value: undefined,
                label: 'select'
            });

            api.stores.list().then((response) => {
                each(response, (store) => {
                    self.dataStores.push({
                        value: store.id,
                        label: store.name
                    });
                });
            });

            state.title = 'schedule:list.title';

            state.navbar.addButton({
                type: 'add',
                viewModel: this,
                permission: 'sentinel://schedule/add'
            });

            state.navbar.addButton({
                type: 'refresh',
                viewModel: this
            });
        },

        add: function () {
            router.goto({
                resource: 'schedule',
                action: 'item'
            });
        },

        refresh: function () {
            this.refreshTimestamp = Date.now();
        }
    });

export default Component.extend({
    tag: 'sentinel-schedule-list',
    ViewModel,
    view
});