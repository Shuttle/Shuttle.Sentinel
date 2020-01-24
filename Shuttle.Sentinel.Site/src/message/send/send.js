import {DefineMap,Component} from 'can';import view from './send.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import localisation from '~/localisation';
import Api from 'shuttle-can-api';
import validator from 'can-define-validate-validatejs';
import state from '~/state';
import {MessageHeaderList} from "./headers";

resources.add('message', {action: 'send', permission: Permissions.Manage.Messages});

const MessageTypeMap = DefineMap.extend({
    messageType: {
        type: 'string',
        default: ''
    },
    emptyMessageType: {
        type: 'string',
        default: ''
    }
});

var api = {
    messages: new Api({
        endpoint: 'messages'
    }),
    messageTypes: new Api({
        endpoint: 'messagetypes/{search}',
        Map: MessageTypeMap
    })
}

export const ViewModel = DefineMap.extend({
    destinationQueueUri: {
        type: 'string',
        default: '',
        validate: {
            presence: true
        }
    },

    message: {
        type: 'string',
        default: '',
        validate: {
            presence: true
        }
    },

    messageType: {
        type: 'string',
        default: '',
        validate: {
            presence: true
        }
    },

    init() {
        state.title = 'message:title-send';
    },

    searchValue: {
        type: 'string',
        default: ''
    },

    get list() {
        return api.messageTypes.list({search: encodeURIComponent(this.searchValue)});
    },

    search: function (el) {
        this.searchValue = el.value;

        $(el).dropdown();
    },

    select: function (item) {
        this.messageType = item.messageType;
        this.message = item.emptyMessageType;
    },

    send() {
        const self = this;

        if (!!this.errors()) {
            return false;
        }

        api.messages.post({
            destinationQueueUri: this.destinationQueueUri,
            messageType: this.messageType,
            message: this.message,
            headers: this.headers.serialize()
        })
            .then(function () {
                state.alerts.add({message: localisation.value('message:sent')});
            });

        return false;
    },

    headers: {
        Type: MessageHeaderList
    }
});

validator(ViewModel);

export default Component.extend({
    tag: 'sentinel-message-send',
    ViewModel,
    view
});