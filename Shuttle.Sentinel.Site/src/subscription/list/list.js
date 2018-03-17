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

resources.add('subscription', {action: 'list', permission: Permissions.Manage.Subscriptions});

const Subscription = DefineMap.extend({
    messageType: {
        type: 'string',
        default: ''
    },

    inboxWorkQueueUri: {
        type: 'string',
        default: ''
    },

    remove() {
        const serialized = this.serialize();

        serialized.dataStoreId = this.dataStoreId;

        remove.post(serialized)
            .then(function () {
                state.alerts.show({
                    message: localisation.value('itemRemovalRequested', {itemName: localisation.value('subscription:title')}),
                    name: 'item-removal'
                });
            });
    },

    clone: function () {
        const serialized = this.serialize();

        serialized.dataStoreId = this.dataStoreId;

        state.put('subscription', serialized);

        router.goto({
            resource: 'subscription',
            action: 'add'
        });
    }
});

var api = {
    subscriptions: new Api({
        endpoint: 'subscriptions/{id}',
        Map: Subscription
    }),
    remove: new Api({
        endpoint: 'subscriptions/remove'
    }),
    stores: new Api({
        endpoint: 'datastores'
    })
}


export const ViewModel = DefineMap.extend(
    'subscriptions',
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

        get subscriptions() {
            const refreshTimestamp = this.refreshTimestamp;
            const dataStoreId = this.dataStoreId;

            return !dataStoreId ? undefined : api.subscriptions.list({id: dataStoreId});
        },

        init() {
            const self = this;
            const columns = this.columns;

            if (!columns.length) {
                columns.push({
                    columnTitle: 'clone',
                    columnClass: 'col-1',
                    stache: '<cs-button text:from="\'clone\'" click:from="@clone" elementClass:from="\'btn-sm\'"/>'
                });

                columns.push({
                    columnTitle: 'subscription:message-type',
                    columnClass: 'col-3',
                    attributeName: 'messageType'
                });

                columns.push({
                    columnTitle: 'subscription:inbox-work-queue-uri',
                    columnClass: 'col',
                    attributeName: 'securedUri'
                });

                columns.push({
                    columnTitle: 'remove',
                    columnClass: 'col-1',
                    stache: '<cs-button-remove click:from="@remove" elementClass:from="\'btn-sm\'"/>'
                });
            }

            api.stores.list().then((response) => {
                each(response, (store) => {
                    self.dataStores.push({
                        value: store.id,
                        label: store.name
                    });
                });
            });

            state.title = localisation.value('subscription:list.title');

            state.navbar.addButton({
                type: 'add',
                viewModel: this,
                permission: 'sentinel://subscription/add'
            });

            state.navbar.addButton({
                type: 'refresh',
                viewModel: this
            });
        },

        add: function () {
            router.goto('subscription/add');
        },

        refresh: function () {
            this.refreshTimestamp = Date.now();
        }
    });

export default Component.extend({
    tag: 'sentinel-subscription-list',
    ViewModel,
    view
});