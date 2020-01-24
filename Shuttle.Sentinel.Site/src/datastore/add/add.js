import {DefineMap,Component} from 'can';
import view from './add.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import router from '~/router';
import Api from 'shuttle-can-api';
import validator from 'can-define-validate-validatejs';
import state from '~/state';
import stack from '~/stack';

resources.add('datastore', {action: 'add', permission: Permissions.Manage.DataStores});

var datastores = new Api({
    endpoint: 'datastores/{id}'
});

export const ViewModel = DefineMap.extend({
    init: function () {
        const result = stack.pop('datastore');

        state.title = 'datastore:add.title';

        if (!result) {
            return;
        }

        this.name = result.name;
        this.connectionString = result.connectionString;
        this.providerName = result.providerName;
    },

    name: {
        type: 'string',
        default: '',
        validate: {
            presence: true
        }
    },

    connectionString: {
        type: 'string',
        default: '',
        validate: {
            presence: true
        }
    },

    providerName: {
        type: 'string',
        default: '',
        validate: {
            presence: true
        }
    },

    add: function () {
        if (!!this.errors()) {
            return false;
        }

        datastores.post({
            name: this.name,
            connectionString: this.connectionString,
            providerName: this.providerName
        });

        this.close();

        return false;
    },

    close: function () {
        router.goto({
            resource: 'datastore',
            action: 'list'
        });
    }
});

validator(ViewModel);

export default Component.extend({
    tag: 'sentinel-datastore-add',
    ViewModel,
    view
});