import Component from 'can-component';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import view from './queue.stache!';
import Queue from '~/models/queue';

export const ViewModel = DefineMap.extend({
    value: {type: 'string'},

    uri: { type: 'string' },

    get queuesPromise() {
        return Queue.getList({ search: this.uri });
    },

    showQueues: function() {
        this.uri = '';
    },

    searchQueues: function(el) {
        this.uri = el.value;

        $(el).parent().addClass('open');
    },

    _ignoreClick: function(ev) {
        ev.stopPropagation();
    },

    selectQueue: function(queue) {
        this.value = queue.uri;
    }
});

export default Component.extend({
    tag: 'sentinel-queue',
    ViewModel,
    view
});