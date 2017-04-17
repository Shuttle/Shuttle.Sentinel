import $ from 'jquery';
import DefineMap from 'can-define/map/';
import resources from 'sentinel/resources';
import localisation from 'sentinel/localisation';
import security from 'sentinel/security';
import alerts from 'sentinel/alerts';
import navigationItems from 'sentinel/navigation-map';
import guard from 'sentinel/guard';
import logger from 'sentinel/logger';

var State = DefineMap.extend({
    data: {
        value: Map()
    },
    alerts: {
        value: new can.List()
    },
    permissions: {
        value: new can.List()
    },
    token: {
        value: undefined
    },
    route: {
        value: new Map()
    },
    loginStatus: {
        get: function() {
            return this.isUserRequired ? 'user-required' : this.attr('token') == undefined ? 'not-logged-in' : 'logged-in';
        }
    },
    username: {
        value: undefined
    },
    navigationItems: {
        get: function() {
            var result = new can.List();

            can.each(navigationItems, function(item) {
                var add = false;
                var navigationItem = new can.Map({
                    href: item.href,
                    text: item.text,
                    items: new can.List()
                });

                if (!item.permission || security.hasPermission(item.permission)) {
                    if (item.items !== undefined) {
                        can.each(item.items, function(subitem) {
                            if (!subitem.permission || security.hasPermission(subitem.permission)) {
                                add = true;

                                navigationItem.attr('items').push(new can.Map({
                                    href: subitem.href,
                                    text: subitem.text
                                }));
                            }
                        });
                    } else {
                        add = true;
                    }

                    if (add) {
                        result.push(navigationItem);
                    }
                }
            });

            return result;
        }
    },
    modal: {
        value: new Map({
            confirmation: new Map({
                message: 'hello'
            })
        })
    },

    init: function () {
        var self = this;

        this.route.delegate('*', 'change', function () {
            self.handleRoute.call(self);
        });
    
        $.ajaxPrefilter(function( options, originalOptions, jqXHR ) {
            options.error = function (xhr) {
                if (xhr.responseJSON) {
                    alerts.show({ message: xhr.responseJSON.message, type: 'danger', name: 'ajax-prefilter-error' });
                } else {
                    alerts.show({ message: xhr.status + ' / ' + xhr.statusText, type: 'danger', name: 'ajax-prefilter-error' });
                }

                if (originalOptions.error) {
                    originalOptions.error(xhr);
                }            }
        });
    },

    handleRoute: function () {
        var resource;
        var resourceName = this.route.attr('resource');
        var actionName = this.route.attr('action');
        var isActionRoute = !!actionName;
        var previousHash = this.attr('previousHash');

        if ($('#application-content').length === 0) {
            return;
        }

        if (!resourceName) {
            return;
        }

        if (isActionRoute) {
            if (!actionName) {
                return;
            }

            resource = resources.find(resourceName, { action: actionName });
        } else {
            resource = resources.find(resourceName);
        }

        if (previousHash && previousHash === window.location.hash) {
            return;
        }

        this.attr('previousHash', window.location.hash);

        if (!resource) {
            alerts.show({ message: localisation.value('exceptions.resource-not-found', { hash: window.location.hash, interpolation: { escape: false } }), type: 'warning', name: 'route-error' });

            return;
        }

        if (resource.permission && !security.hasPermission(resource.permission)) {
            alerts.show({ message: localisation.value('security.access-denied', { name: resource.name || window.location.hash, permission: resource.permission, interpolation: { escape: false } }), type: 'danger', name: 'route-error' });
	    
            return;
        }

        alerts.clear();

        var componentName = resource.componentName || 'sentinel-' + resource.name + (isActionRoute ? '-' + actionName : '');

        $('#application-content').html(can.stache('<' + componentName + '></' + componentName + '>')(this));
    },

    set: function(name, data) {
        guard.againstUndefined(name, 'name');

        this.attr('data.' + name, data);
    },

    get: function(name) {
        guard.againstUndefined(name, 'name');
        
        let key = 'data.' + name;
        let previousKey = this.attr('data.__previousKey');
        let result = this.attr(key);

        if (result) {
            this.removeAttr(key);
        } else {
            if (key === previousKey) {
                logger.info('There is no data item available for key \'' + key + '\'.  However, your last access was to this key.  Keep in mind that when you call \'get\' the data item is destroyed.  To re-use it you wil need to place it in a local variable.');
            }
        }

        this.attr('data.__previousKey', key);

        return result;
    },

    goto: function(href) {
        window.location.hash = (href.indexOf('#!') === -1 ? '#!' : '') + href;
    }
});

export default new State();