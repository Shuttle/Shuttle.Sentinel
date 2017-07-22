import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import view from './list.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import router from '~/router';
import Api from '~/api';
import alerts from '~/alerts';
import localisation from '~/localisation';
import state from '~/state';
import each from 'can-util/js/each/';

resources.add('subscription', { action: 'list', permission: Permissions.Manage.Subscriptions });

const Subscription = DefineMap.extend({
    messageType: { type: 'string', value: '' },
    inboxWorkQueueUri: { type: 'string', value: '' }
});

var subscriptions = new Api({
    endpoint: 'subscriptions/{id}',
    Map: Subscription
});

var dataStores = new Api('datastores');

export const ViewModel = DefineMap.extend(
    'subscriptions',
    {
        columns: { Value: DefineList },
        refreshTimestamp: { type: 'string' },
        dataStoreId: { type: 'string', value: '' },
        dataStores: { Value: DefineList },

        get subscriptions () {
            const refreshTimestamp = this.refreshTimestamp;
            const dataStoreId = this.dataStoreId;

            return !dataStoreId ? undefined : subscriptions.list({ id: dataStoreId });
        },

        init: function() {
            const self = this;
            const columns = this.columns;

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
                    columnTitle: 'subscription:message-type',
                    attributeName: 'messageType'
                });

                columns.push({
                    columnTitle: 'subscription:inbox-work-queue-uri',
                    attributeName: 'securedUri'
                });

                columns.push({
                    columnTitle: 'remove',
                    columnClass: 'col-md-1',
                    columnType: 'remove-button',
                    buttonContext: this,
                    buttonClick: 'remove'
                });
            }

            self.dataStores.push({ value: undefined, label: 'select' });

            dataStores.list().then((response) => {
                each(response, (store) => {
                    self.dataStores.push({
                        value: store.id,
                        label: store.name
                    });
                });
            });
        },

        add: function() {
            router.goto('subscription/add');
        },

        refresh: function() {
            this.refreshTimestamp = Date.now();
        },

        remove: function(row) {
            subscriptions.delete({ id: row.id })
                .then(function() {
                    alerts.show({ message: localisation.value('itemRemovalRequested', { itemName: localisation.value('subscription:title') }), name: 'item-removal' });
                });
        },

        clone: function(row) {
            state.push('subscription', row);

            this.add();
        }
    });

export default Component.extend({
    tag: 'sentinel-subscription-list',
    ViewModel,
    view
});