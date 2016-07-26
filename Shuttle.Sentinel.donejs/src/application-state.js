import can from 'can';
import Map from 'can/map/';
import resources from 'sentinel/resources';
import logger from 'sentinel/logger';
import localisation from 'sentinel/localisation';
import security from 'sentinel/security';
import Permissions from 'sentinel/Permissions';

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
		const callContext = function (ev, prop, change, newVal, oldVal) {
		    self.handleRoute.call(self, ev, prop, change, newVal, oldVal);
	    };
	    this.route.bind('change', callContext);
	},

    handleRoute: function (ev, prop, change, newVal, oldVal) {
		var resource;
		var componentName;
		var resourceName = this.route.attr('resource');
		var actionName = this.route.attr('action');
		var isActionRoute = ev.target.route === ':resource/:action';

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
			logger.error(localisation.value('exceptions.resource-not-found', { hash: window.location.hash }));

		    if (window.location.hash !== '#!dashboard') {
		        window.location.hash = '#!dashboard';
		    }

		    return;
		}

		if (resource.permission && !security.hasPermission(resource.permission)) {
		    security.accessDenied(resource.permission);
		}

		componentName = resource.componentName || 'sentinel-' + resource.name + (isActionRoute ? '-' + actionName : '');

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