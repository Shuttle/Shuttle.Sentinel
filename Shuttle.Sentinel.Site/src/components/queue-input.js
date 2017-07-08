import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './queue-input.stache!';
import Queue from '~/models/queue';

export const ViewModel = DefineMap.extend({
    search: { type: 'string' },
    value: { type: 'string' },
    uri: { type: 'string' },

    get queuesPromise() {
        return Queue.getList({ search: encodeURIComponent(this.search) });
    },

    showQueues: function() {
        this.search = '';
    },

    searchQueues: function(el) {
        this.search = el.value;

        $(el).parent().addClass('open');
    },

    _ignoreClick: function(ev) {
        ev.stopPropagation();
    },

    selectQueue: function(queue) {
        this.value = queue.displayUri;
        this.uri = queue.uri;
    }
});

export default Component.extend({
    tag: 'sentinel-queue-input',
    ViewModel,
    view
});