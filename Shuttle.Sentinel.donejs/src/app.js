import can from 'can';
import template from 'sentinel/index.stache!';
import $ from 'jquery';
import localisation from 'sentinel/localisation';
import security from 'sentinel/security';
import state from 'sentinel/state';
import 'can/map/define/';
import 'can/map/delegate/';
import 'bootstrap/dist/js/bootstrap'

import 'sentinel/components/back-button';
import 'sentinel/components/checkbox';
import 'sentinel/components/refresh-button';
import 'sentinel/components/remove-button';
import 'sentinel/components/working-button';
import 'sentinel/components/button';
import 'sentinel/components/buttons';
import 'sentinel/components/form-group';
import 'sentinel/components/fetching';
import 'sentinel/components/label';
import 'sentinel/components/input';
import 'sentinel/components/text';
import 'sentinel/components/form';
import 'sentinel/components/page-title';
import 'sentinel/components/validation';
import 'sentinel/components/modal';
import 'sentinel/components/table';

import 'sentinel/components/navigation';
import 'sentinel/components/alerts';

import 'validate.js';

import 'sentinel/dashboard/';
import 'sentinel/user/';
import 'sentinel/role/';
import 'sentinel/message/';

localisation.start(function(error) {
    if (error) {
        throw new Error(error);
    }

    security.start()
        .done(function() {
            can.route(':resource');
            can.route(':resource/:action');
            can.route(':resource/:id/:action');

            can.route.map(state.route);

            can.route.ready();
        })
        .always(function() {
            $('#application-container').html(template(state));

            if (window.location.hash === '#!' || !window.location.hash) {
                window.location.hash = state.isUserRequired
                                           ? '#!user/register'
                                           : '#!dashboard';
            }

            state.handleRoute();
        });
});