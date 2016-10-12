import Map from 'can/map/';
import List from 'can/list/';
import 'can/map/define/';
import api from 'sentinel/api';

var _emptyItem = {};

var Model = Map.extend({
    define: {
        list: {
            value: new List()
        },

        item: {
            value: _emptyItem
        },

        isPending: {
            value: true
        },

        isResolved: {
            value: false
        },

        isListResolved: {
            value: false
        },

        isItemResolved: {
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
                deferred.resolve(response.data);
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
        this.attr('isListResolved', false);
        this.attr('isItemResolved', false);
    },

    resolved: function() {
        this.attr('isPending', false);
        this.attr('isResolved', true);
        this.attr('hasFailed', false);
        this.attr('isListResolved', this.attr('list') && !!this.attr('list').length);
        this.attr('isItemResolved', this.attr('item') && (this.attr('item') !== _emptyItem));
    },

    failed: function() {
        this.attr('isPending', false);
        this.attr('isResolved', false);
        this.attr('hasFailed', true);
        this.attr('isListResolved', false);
        this.attr('isItemResolved', false);
    }
});

export default Model;