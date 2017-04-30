import $ from 'jquery';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import api from '~/api';
import guard from '~/guard';

var _emptyItem = {};

var Model = DefineMap.extend({
    list: {
        Value: DefineList
    },

    item: {
        value: _emptyItem
    },

    isPending: {
        type: 'boolean',
        value: true
    },

    isResolved: {
        type: 'boolean',
        value: false
    },

    isListResolved: {
        type: 'boolean',
        value: false
    },

    isItemResolved: {
        type: 'boolean',
        value: false
    },

    hasFailed: {
        type: 'boolean',
        value: false
    },
    
    get: function(endpoint, id) {
        var self = this;
        var deferred = $.Deferred();

        self.pending();

        api.get(endpoint + (!!id ? '/' + id : ''))
            .done(function(response) {
                if (!!id) {
                    self.item = response.data;
                } else {
                    self.list.replace(response.data);
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

    put: function(endpoint, data, options) {
        var o = options || {};
        var self = this;
        var deferred = $.Deferred();

        self.pending();

        o.data = data;

        api.put(endpoint, o)
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
        this.isPending = true;
        this.isResolved = false;
        this.hasFailed = false;
        this.isListResolved = false;
        this.isItemResolved = false;
    },

    resolved: function() {
        this.isPending = false;
        this.isResolved = true;
        this.hasFailed = false;
        this.isListResolved = this.list && !!this.list.length;
        this.isItemResolved = this.item && (this.item !== _emptyItem);
    },

    failed: function() {
        this.isPending = false;
        this.isResolved = false;
        this.hasFailed = true;
        this.isListResolved = false;
        this.isItemResolved = false;
    },

    automap: function(map) {
        var o;
        var attribute;

        guard.againstUndefined(map, 'map');
        guard.againstMissingFunction(map.serialize, 'map.serialize');

        o = map.serialize();

        for (attribute in o) {
            if (!o.hasOwnProperty(attribute)) {
                continue;
            }

            this[attribute] = map[attribute];
        }
    }
});

export default Model;