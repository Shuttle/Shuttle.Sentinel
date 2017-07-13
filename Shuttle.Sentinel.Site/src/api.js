import $ from 'jquery';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import loader from '@loader';
import alerts from '~/alerts';
import guard from '~/guard';
import each from 'can-util/js/each/';

$.ajaxPrefilter(function(options, originalOptions) {
    options.error = function(xhr) {
        if (xhr.responseJSON) {
            alerts.show({ message: xhr.responseJSON.message, type: 'danger', name: 'ajax-prefilter-error' });
        } else {
            alerts.show({ message: xhr.status + ' / ' + xhr.statusText, type: 'danger', name: 'ajax-prefilter-error' });
        }

        if (originalOptions.error) {
            originalOptions.error(xhr);
        }
    };
});

let parameterExpression = /\{.*?\}/g;

let api = function(options) {
    guard.againstUndefined(options, 'options');

    var apiOptions = (typeof options === 'string' || options instanceof String)
        ? { endpoint: options}
        : options;

    guard.againstUndefined(apiOptions.endpoint, 'options.endpoint');

    if (!apiOptions.cache) {
        apiOptions.cache = false;
    }

    const call = {
        execute (options) {
            return new Promise((resolve, reject) => {
                try {
                    const o = options || {};
                    const parsedEndpoint = this.parseEndpoint(apiOptions.endpoint, o.parameters);

                    $.ajax({
                        url: parsedEndpoint.url,
                        type: o.method,
                        async: true,
                        cache: apiOptions.cache,
                        dataType: 'json',
                        contentType: 'application/json',
                        data: JSON.stringify(o.data || {}),
                        beforeSend: o.beforeSend,
                        timeout: o.timeout || 60000
                    })
                        .done(function(response) {
                            resolve(response);
                        })
                        .fail(function(jqXHR, textStatus, errorThrown) {
                            reject(new Error(errorThrown));
                        });
                } catch (e) {
                    reject(e);
                }
            });
        },

        parseEndpoint (endpoint, parameters) {
            guard.againstUndefined(endpoint, 'endpoint');

            const p = parameters || {};
            const params = [];
            let match;

            do {
                match = parameterExpression.exec(endpoint);

                if (match) {
                    const name = match[0];

                    if (name.length < 3) {
                        throw new Error($
                            `Endpoint '{endpoint}' contains parameter '{name}' that is not at least 3 characters in length.`);
                    }

                    params.push({
                        name: name.substr(1, name.length - 2),
                        index: match.index
                    });
                }
            } while (match);

            let url = endpoint.indexOf('http') < 0 ? loader.serviceBaseURL + endpoint : endpoint;

            each(params,
                function(param) {
                    if (!p[param]) {
                        throw new Error(`No parameter value specified for parameter '{${param}}'.`);
                    }

                    url = s.replace(`{${param}}`, p[param]);
                });

            return {
                url: url,
                parameters: params
            };
        }
    };

    return {
        post (data) {
            guard.againstUndefined(data, 'data');

            return call.execute({
                data: data,
                method: 'POST'
            });
        },

        put (data) {
            guard.againstUndefined(data, 'data');

            return call.execute({
                data: data,
                method: 'POST'
            });
        },

        item (parameters) {
            return call.execute({
                method: 'GET',
                parameters: parameters
            })
                .then(function(response) {
                    return !!apiOptions.Map
                        ? new apiOptions.Map(response)
                        : new DefineMap(response);
                });
        },

        list (parameters) {
            return call.execute({
                method: 'GET',
                parameters: parameters
            })
                .then(function(response) {
                    const result = !!apiOptions.List
                        ? new apiOptions.List()
                        : new DefineList();

                    each(response.data, (item) => {
                        result.push(!!apiOptions.Map
                            ? new apiOptions.Map(item)
                            : new DefineMap(item));
                    });

                    return result;
                });
        },

        'delete' (parameters) {
            guard.againstUndefined(parameters, 'parameters');

            return call.execute({
                data: data,
                method: 'DELETE',
                parameters: parameters
            });
        }
    };
};

export default api;