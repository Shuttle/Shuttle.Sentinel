import template from '~/main.stache!';
import $ from 'jquery';
import localisation from '~/localisation';
import security from '~/security';
import state from '~/state';
import route from 'can-route';
import router from '~/router';

import '~/components/alerts';
import '~/components/page-title';

import '~/dashboard/';

localisation.start(function(error) {
    if (error) {
        throw new Error(error);
    }

    security.start()
        .done(function() {
            route('{resource}');
            route('{resource}/{action}');
            route('{resource}/{id}/{action}');

            route.data = router.route;

            route.ready();
        })
        .always(function() {
            $('#application-container').html(template(state));

            if (window.location.hash === '#!' || !window.location.hash) {
                window.location.hash = security.isUserRequired
                                           ? '#!user/register'
                                           : '#!dashboard';
            }

            router.process();
        });
});




