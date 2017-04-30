import $ from 'jquery';
import configuration from '~/configuration';
import alerts from '~/alerts';

$.ajaxPrefilter(function( options, originalOptions, jqXHR ) {
    options.error = function(xhr) {
        if (xhr.responseJSON) {
            alerts.show({ message: xhr.responseJSON.message, type: 'danger', name: 'ajax-prefilter-error' });
        } else {
            alerts.show({ message: xhr.status + ' / ' + xhr.statusText, type: 'danger', name: 'ajax-prefilter-error' });
        }

        if (originalOptions.error) {
            originalOptions.error(xhr);
        }
    }
});

let api = {
    post: function(endpoint, options) {
        const o = options || {};

        if (o.async == undefined) {
            o.async = true;
        }

        var data = o.data || {};

        return $.ajax({
            url: this.url(endpoint),
            type: 'POST',
            async: o.async,
            contentType: 'application/json',
            data: JSON.stringify(data),
            beforeSend: o.beforeSend,
            timeout: o.timeout || 60000
        });
    },

    put: function(endpoint, options) {
        const o = options || {};

        if (o.async == undefined) {
            o.async = true;
        }

        var data = o.data || {};

        return $.ajax({
            url: this.url(endpoint),
            type: 'PUT',
            async: o.async,
            contentType: 'application/json',
            data: JSON.stringify(data),
            beforeSend: o.beforeSend,
            timeout: o.timeout || 60000
        });
    },

    get: function (endpoint, options) {
        var o = options || {};

        if (o.async == undefined) {
            o.async = true;
        }

        if (!o.cache) {
            o.cache = false;
        }

        return $.ajax({
            url: this.url(endpoint),
            dataType: 'json',
            type: 'GET',
            async: o.async,
            cache: o.cache,
            timeout: o.timeout || 60000
        });
    },

    'delete': function (endpoint, options) {
        var o = options || {};

        if (o.async == undefined) {
            o.async = true;
        }

        return $.ajax({
            url: this.url(endpoint),
            type: 'DELETE',
            async: o.async,
            timeout: o.timeout || 60000
        });
    },

    url: function(endpoint) {
        return endpoint.indexOf('http') < 0 ? configuration.controllerUrl(endpoint) : endpoint;
    }
};

export default api;