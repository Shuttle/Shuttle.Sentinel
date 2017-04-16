import template from "~/main.stache!";
import $ from 'jquery';
import DefineMap from 'can-define/map/map';
import configuration from '~/configuration';
import localisation from '~/localisation';

localisation.start(function(error) {
    if (error) {
        throw new Error(error);
    }

    //security.start()
    //    .done(function() {
    //        can.route(':resource');
    //        can.route(':resource/:action');
    //        can.route(':resource/:id/:action');

    //        can.route.map(state.route);

    //        can.route.ready();
    //    })
    //    .always(function() {
    //        $('#application-container').html(template(state));

    //        if (window.location.hash === '#!' || !window.location.hash) {
    //            window.location.hash = state.isUserRequired
    //                                       ? '#!user/register'
    //                                       : '#!dashboard';
    //        }

    //        state.handleRoute();
    //    });

    document.body.appendChild(template(new DefineMap({message: 'Hello World'})));
});




