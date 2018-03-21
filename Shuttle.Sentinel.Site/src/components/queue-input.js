import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './queue-input.stache!';
import Api from 'shuttle-can-api';
import ComponentViewModel from 'shuttle-canstrap/infrastructure/component-view-model';

const Queue = DefineMap.extend(
    'queue',
    {
        seal: false
    },
    {
        uri: {
            type: 'string'
        },
        securedUri: {
            type: 'string'
        }
    }
);

var queues = new Api({
    endpoint: 'queues/{search}',
    Map: Queue
});

export const ViewModel = ComponentViewModel.extend({
    search: {
        type: 'string',
        default: ''
    },

    uri: {
        type: 'string',
        default: ''
    },

    get queuesPromise() {
        return queues.list({search: encodeURIComponent(this.search)});
    },

    showQueues: function () {
        this.search = '';
    },

    searchQueues: function (el) {
        this.search = el.value;
        this.uri = el.value;
        this.value = el.value;

        $(el).dropdown();
    },

    selectQueue: function (queue) {
        this.value = queue.securedUri;
        this.uri = queue.uri;
    }
});

export default Component.extend({
    tag: 'cs-queue-input',
    ViewModel,
    view
});