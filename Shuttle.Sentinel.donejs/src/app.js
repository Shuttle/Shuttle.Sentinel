import can from 'can';
import template from 'sentinel/index.stache!';
import $ from 'jquery';
import localisation from 'sentinel/localisation';
import security from 'sentinel/security';
import state from 'sentinel/application-state';
import Permissions from 'sentinel/Permissions';

import 'sentinel/dashboard/';
import 'sentinel/user/';

import 'sentinel/components/button';
import 'sentinel/components/container';
import 'sentinel/components/label';
import 'sentinel/components/input';
import 'sentinel/components/text';


localisation.start(function(error) {
    if (error) {
        throw new Error(error);
    }

    security.fetchAnonymousPermissions()
        .done(function() {
            can.route(':resource');
            can.route(':resource/:action');

            can.route.map(state.route);

            state.attr('loginStatus', security.hasPermission(Permissions.States.UserRequired) ? 'user-required' : 'not-logged-in');

            $('#application-container').html(template, state);

            can.route.ready();

            window.location.hash = security.hasPermission(Permissions.States.UserRequired)
                                       ? '#!user/register'
                                       : '#!dashboard';
        })
        .fail(function(error) {
            alert('[TODO: proper page] / ERROR : ' + error);
        });
});
