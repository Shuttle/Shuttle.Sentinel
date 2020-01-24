import {DefineMap,Component} from 'can';
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
    searchValue: {
        type: 'string',
        default: ''
    },

    get queuesPromise() {
        return queues.list({search: encodeURIComponent(this.searchValue)});
    },

    showQueues: function () {
        this.search = '';
    },

    searchQueues: function (el) {   
        this.searchValue = el.value;
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
    tag: 'sentinel-queue-input',
    ViewModel,
    view
});