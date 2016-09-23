import Map from 'can/map/';
import 'can/map/define/';
import api from 'sentinel/api';

var Item = Map.extend({
    define: {
        isPending: {
            value: true
        },

        isResolved: {
            value: false
        }
    },
    
    post: function(endpoint, data, options) {
        var o = options || {};
        var self = this;

        self.pending();

        o.data = data;

        api.post(endpoint, o)
            .done(function() {
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

export default Item;