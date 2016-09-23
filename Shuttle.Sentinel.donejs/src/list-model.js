import Map from 'can/map/';
import 'can/map/define/';
import api from 'sentinel/api';

var List = Map.extend({
    define: {
        list: {
            value: new can.List()
        },

        isPending: {
            value: true
        },

        isResolved: {
            value: false
        }
    },
    
    fetch: function(endpoint) {
        var self = this;

        self.pending();

        api.get(endpoint)
            .done(function(response) {
                self.attr('list').replace(response.data);

                self.resolved();
            });
    },

    pending: function() {
        this.attr('isPending', true);
        this.attr('isResolved', false);
    },

    resolved: function() {
        this.attr('isPending', false);
        this.attr('isResolved', true);
    }
});

export default List;