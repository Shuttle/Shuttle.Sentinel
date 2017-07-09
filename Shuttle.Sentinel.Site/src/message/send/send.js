import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import view from './send.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import alerts from '~/alerts';
import localisation from '~/localisation';
import api from '~/api';
import each from 'can-util/js/each/';
import $ from 'jquery';
import Message from '~/models/message-send';
import validator from 'can-define-validate-validatejs';

resources.add('message', { action: 'send', permission: Permissions.Manage.Messages });

export const ViewModel = DefineMap.extend({
    destinationQueueUri: {
        type: 'string',
        value: '',
        validate: {
            presence: true
        }
    },

    message: {
        type: 'string',
        value: '',
        validate: {
            presence: true
        }
    },

    messageType: {
        type: 'string',
        value: '',
        validate: {
            presence: true
        }
    },

    send () {
        const self = this;

        if (!!this.errors()) {
            return false;
        }

        alert('send');

        return false;
    }
});

validator(ViewModel);

export default Component.extend({
    tag: 'sentinel-message-send',
    ViewModel,
    view
});