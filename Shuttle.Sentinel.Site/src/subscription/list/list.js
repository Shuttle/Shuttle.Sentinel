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

resources.add('subscription', { action: 'list', permission: Permissions.Manage.Subscriptions });

var subscriptions = new Api('subscriptions/{id}');

export const ViewModel = DefineMap.extend({
    columns: { Value: DefineList },
    refreshTimestamp: { type: 'string' },

    get list () {
        const refreshTimestamp = this.refreshTimestamp;
        return subscriptions.list();
    },

    init: function() {
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
                attributeName: 'inboxWorkQueueUri'
            });

            columns.push({
                columnTitle: 'remove',
                columnClass: 'col-md-1',
                columnType: 'remove-button',
                buttonContext: this,
                buttonClick: 'remove'
            });
        }
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