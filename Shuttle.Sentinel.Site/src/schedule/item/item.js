import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import resources from '~/resources';
import Permissions from '~/permissions';
import view from './item.stache!';
import router from '~/router';
import Api from 'shuttle-can-api';
import validator from 'can-define-validate-validatejs';
import state from '~/state';
import stack from '~/stack';
import moment from "moment/moment";
import {OptionList} from "shuttle-canstrap/select/";
import each from 'can-util/js/each/';

resources.add('schedule', {action: 'item', permission: Permissions.Manage.Schedules});

var api = {
    dataStores : new Api({
        endpoint: 'datastores'
    }),
    schedules: new Api({
        endpoint: 'schedules/{dataStoreId}/{id}'
    })
}

export const ViewModel = DefineMap.extend({
    dataStores: {
        Default: OptionList
    },

    init() {
        const self = this;
        const result = stack.pop('schedule');

        state.title = 'schedule:item.title';

        self.dataStores.push({
            value: undefined,
            label: 'select'
        });

        api.dataStores.list().then((response) => {
            each(response, (store) => {
                self.dataStores.push({
                    value: store.id,
                    label: store.name
                });
            });
        });

        if (!result) {
            return;
        }

        this.dataStoreId = result.dataStoreId;
        this.id = result.id;
        this.name = result.name;
        this.inboxWorkQueueUri = result.inboxWorkQueueUri;
        this.cronExpression = result.cronExpression;
        this.nextNotification = result.nextNotification;
    },

    dataStoreId: {
        type: 'string',
        default: '',
        validate: {
            presence: true
        }
    },

    id: {
        type: 'string',
        default: ''
    },

    name: {
        type: 'string',
        default: '',
        validate: {
            presence: true
        }
    },

    inboxWorkQueueUri: {
        type: 'string',
        default: '',
        validate: {
            presence: true
        }
    },

    cronExpression: {
        type: 'string',
        default: '',
        validate: {
            presence: true
        }
    },

    nextNotification: {
        type: 'date',
        default: '',
        validate: {
            date: true
        }
    },

    save() {
        if (!!this.errors()) {
            return false;
        }

        api.schedules.post({
            dataStoreId: this.dataStoreId,
            id: this.id,
            name: this.name,
            inboxWorkQueueUri: this.inboxWorkQueueUri,
            cronExpression: this.cronExpression,
            nextNotification: this.nextNotification
        });

        this.close();

        return false;
    },

    close: function () {
        router.goto({
            resource: 'schedule',
            action: 'list'
        });
    }
});

validator(ViewModel);

export default Component.extend({
    tag: 'sentinel-schedule-item',
    ViewModel,
    view
});