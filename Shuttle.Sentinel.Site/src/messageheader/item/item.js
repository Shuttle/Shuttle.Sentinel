import {DefineMap,Component} from 'can';
import resources from '~/resources';
import Permissions from '~/permissions';
import view from './item.stache!';
import router from '~/router';
import Api from 'shuttle-can-api';
import validator from 'can-define-validate-validatejs';
import state from '~/state';
import stack from '~/stack';

resources.add('messageheader', {action: 'item', permission: Permissions.Manage.Messages});

var api = new Api({
    endpoint: 'messageheaders/{id}'
});

export const ViewModel = DefineMap.extend({
    init() {
        const result = stack.pop('messageheader');

        state.title = 'messageheader:item.title';

        if (!result) {
            return;
        }

        this.headerKey = result.key;
        this.headerValue = result.value;
    },

    headerKey: {
        type: 'string',
        default: '',
        validate: {
            presence: true
        }
    },

    headerValue: {
        type: 'string',
        default: '',
        validate: {
            presence: true
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
    tag: 'sentinel-messageheader-item',
    ViewModel,
    view
});