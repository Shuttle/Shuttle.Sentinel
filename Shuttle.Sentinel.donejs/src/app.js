import can from 'can';
import template from 'sentinel/index.stache!';
import $ from 'jquery';
import localisation from 'sentinel/localisation';
import security from 'sentinel/security';
import state from 'sentinel/state';
import 'bootstrap/dist/js/bootstrap'

import 'sentinel/dashboard/';
import 'sentinel/user/';

import 'sentinel/components/button';
import 'sentinel/components/container';
import 'sentinel/components/label';
import 'sentinel/components/input';
import 'sentinel/components/text';
import 'sentinel/components/form';
import 'sentinel/components/page-title';

import 'sentinel/components/navigation';
import 'sentinel/components/alerts';

localisation.start(function(error) {
    if (error) {
        throw new Error(error);
    }

    security.start()
        .done(function() {
            can.route(':resource');
            can.route(':resource/:action');
            can.route(':resource/:action/:id');

            can.route.map(state.route);

            can.route.ready();
        })
        .always(function() {
            var hash = window.location.hash;

            $('#application-container').html(template, state);

            window.location.hash = state.attr('loginStatus') === 'user-required'
                                       ? '#!user/register'
                                       : '#!dashboard';

            if (hash === window.location.hash) {
                state.handleRoute();
            }
        });
});