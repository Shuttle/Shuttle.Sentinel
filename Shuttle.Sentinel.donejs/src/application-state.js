import can from 'can';
import Map from 'can/map/';
import resources from 'sentinel/resources';
import logger from 'sentinel/logger';
import localisation from 'sentinel/localisation';
import security from 'sentinel/security';
import Permissions from 'sentinel/Permissions';
import alerts from 'sentinel/alerts';

var State = Map.extend({
    define: {
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
                return security.hasPermission(Permissions.States.UserRequired) ? 'user-required' : this.attr('token') == undefined ? 'not-logged-in' : 'logged-in';
            }
        },
        username: {
            value: undefined
        }
    },

	init: function () {
		var self = this;

		this.route.bind('change', function (ev, prop, change, newVal, oldVal) {
		    self.handleRoute.call(self, ev, prop, change, newVal, oldVal);
		});
	},

	handleRoute: function (ev, prop, change, newVal, oldVal) {
		var resource;
        var resourceName = this.route.attr('resource');
		var actionName = this.route.attr('action');
		var isActionRoute = !!actionName;

        if (!resourceName) {
			return;
		}

		if (isActionRoute) {
			if (prop === 'resource') {
				return;
			}

			resource = resources.find(resourceName, { action: actionName });
		} else {
			resource = resources.find(resourceName);
		}

		if (!resource) {
            alerts.show({ message: localisation.value('exceptions.resource-not-found', { hash: window.location.hash, interpolation: { escape: false } }), type: 'warning', name: 'route-error' });

		    return;
		}

		if (resource.permission && !security.hasPermission(resource.permission)) {
		    alerts.show({ message: localisation.value('security.access-denied', { name: resource.name || window.location.hash, permission: resource.permission, interpolation: { escape: false } }), type: 'danger', name: 'route-error' });
	    
		    return;
		}

	    alerts.remove({ name: 'route-error' });

		var componentName = resource.componentName || 'sentinel-' + resource.name + (isActionRoute ? '-' + actionName : '');

		$('#application-content').html(can.stache('<' + componentName + '></' + componentName + '>')(this));
    },

    userLoggedIn: function(username, token) {
        this.attr('username', username);
        this.attr('token', token);
    },

    userLoggedOut: function() {
        this.attr('username', undefined);
        this.attr('token', undefined);
    }
});

export default new State();