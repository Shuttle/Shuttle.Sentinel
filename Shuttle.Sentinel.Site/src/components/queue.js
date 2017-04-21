import can from 'can';
import Map from 'can/map/';
import List from 'can/list/';
import template from './queue.stache!';
import api from 'sentinel/api';

export const ViewModel = Map.extend({
    define: {
        queues: {
            Value: List
        }
    },

    showQueues: function() {
        this.fetchQueues('');
    },

    searchQueues: function(el) {
        const uri = el.value;

        this.attr('value', uri);

        this.fetchQueues(uri)
            .done(function() {
                $(el).parent().addClass('open');
            });
    },

    fetchQueues: function(uri) {
        const queues = new List();

        this.attr('queues', queues);

        return api.post('queues/search', { data: { uri: uri } })
            .done(function(response) {
                can.each(response.data, function(item) {
                    queues.push(item.uri);
                });
            });
    },

    _ignoreClick: function(ev) {
        ev.stopPropagation();
    },

    selectQueue: function(uri) {
        this.attr('value', uri);
    }
});

export default can.Component.extend({
    tag: 'sentinel-queue',
    template,
    viewModel: ViewModel
});