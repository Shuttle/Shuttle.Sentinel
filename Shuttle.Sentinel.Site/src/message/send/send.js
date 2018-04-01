import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import view from './send.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import localisation from '~/localisation';
import Api from 'shuttle-can-api';
import validator from 'can-define-validate-validatejs';
import state from '~/state';

resources.add('message', {action: 'send', permission: Permissions.Manage.Messages});

const MessageType = DefineMap.extend(
    {
        messageType: {
            type: 'string',
            default: ''
        },
        emptyMessageType: {
            type: 'string',
            default: ''
        }
    }
);

var api = {
    messages: new Api({
        endpoint: 'messages'
    }),
    messageTypes: new Api({
        endpoint: 'messagetypes/{search}',
        Map: MessageType
    })
}

export const ViewModel = DefineMap.extend({
    value: {
        Type: MessageType,
        set(item){
            this.messageType = item.messageType;
            this.message = item.emptyMessageType;
        }
    },

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
        state.title = localisation.value('message:title-send');
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
        this.value = el.value;

        $(el).dropdown();
    },

    select: function (item) {
        this.value = item;
    },

    send() {
        const self = this;

        if (!!this.errors()) {
            return false;
        }

        api.messages.post({
            destinationQueueUri: this.destinationQueueUri,
            messageType: this.messageType,
            message: this.message
        })
            .then(function () {
                state.alerts.show({message: localisation.value('message:sent')});
            });

        return false;
    }
});

validator(ViewModel);

export default Component.extend({
    tag: 'sentinel-message-send',
    ViewModel,
    view
});