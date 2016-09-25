import Map from 'can/map/';
import 'can/map/define/';
import api from 'sentinel/api';

var Model = Map.extend({
    define: {
        list: {
            value: new can.List()
        },

        item: {
            value: new can.Map()  
        },

        isPending: {
            value: true
        },

        isResolved: {
            value: false
        },

        hasFailed: {
            value: false
        }
    },
    
    get: function(endpoint, id) {
        var self = this;
        var deferred = $.Deferred();

        self.pending();

        api.get(endpoint + (!!id ? '/' + id : ''))
            .done(function(response) {
                if (!!id) {
                    self.attr('item', response.data);
                } else {
                    self.attr('list').replace(response.data);
                }

                self.resolved();
                deferred.resolve(response);
            })
            .fail(function() {
                self.failed();
                deferred.reject();
            });

        return deferred;
    },

    post: function(endpoint, data, options) {
        var o = options || {};
        var self = this;
        var deferred = $.Deferred();

        self.pending();

        o.data = data;

        api.post(endpoint, o)
            .done(function() {
                self.resolved();
                deferred.resolve();
            })
            .fail(function() {
                self.failed();
                deferred.reject();
            });

        return deferred;
    },

    'delete': function(endpoint, data, options) {
        var self = this;
        var deferred = $.Deferred();

        self.pending();

        api.delete(endpoint, options)
            .done(function() {
                self.resolved();
                deferred.resolve();
            })
            .fail(function() {
                self.failed();
                deferred.reject();
            });

        return deferred;
    },

    pending: function() {
        this.attr('isPending', true);
        this.attr('isResolved', false);
        this.attr('hasFailed', false);
    },

    resolved: function() {
        this.attr('isPending', false);
        this.attr('isResolved', true);
        this.attr('hasFailed', false);
    },

    failed: function() {
        this.attr('isPending', false);
        this.attr('isResolved', false);
        this.attr('hasFailed', true);
    }
});

export default Model;