import can from 'can';
import template from 'sentinel/index.stache!';
import $ from 'jquery';
import localisation from 'sentinel/localisation';
import security from 'sentinel/security';
import state from 'sentinel/application-state';
import 'bootstrap/dist/js/bootstrap'

import 'sentinel/dashboard/';
import 'sentinel/user/';

import 'sentinel/components/button';
import 'sentinel/components/container';
import 'sentinel/components/label';
import 'sentinel/components/input';
import 'sentinel/components/text';
import 'sentinel/components/form';

import 'sentinel/components/navigation.js';

localisation.start(function(error) {
    if (error) {
        throw new Error(error);
    }

    security.fetchAnonymousPermissions()
        .done(function() {
            can.route(':resource');
            can.route(':resource/:action');

            can.route.map(state.route);

            $('#application-container').html(template, state);

            can.route.ready();

            window.location.hash = state.attr('loginStatus') === 'user-required'
                                       ? '#!user/register'
                                       : '#!dashboard';
        })
        .fail(function(e) {
            alert('[TODO: proper page] / ERROR : ' + e);
        });
});
