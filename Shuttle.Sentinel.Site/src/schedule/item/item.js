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
    init() {
        const result = stack.pop('schedule');

        state.title = 'schedule:item.title';

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

        api.post({
            key: this.headerKey,
            value: this.headerValue,
        });

        this.close();

        return false;
    },

    close: function () {
        router.goto({
            resource: 'messageheader',
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