﻿import stache from '~/main.stache!';
import $ from 'jquery';
import localisation from '~/localisation';
import security from '~/security';
import state from '~/state';
import router from '~/router';
import route from 'can-route';

import '~/components/alerts';
import '~/components/form';
import '~/components/form-group';
import '~/components/input';
import '~/components/label';
import '~/components/navigation';
import '~/components/page-title';
import '~/components/submit-button';
import '~/components/text';
import '~/components/validation';

import '~/dashboard/';
import '~/user/';

localisation.start(function(error) {
    if (error) {
        throw new Error(error);
    }

    security.start()
        .done(function() {
            route('{resource}');
            route('{resource}/{action}');
            route('{resource}/{id}/{action}');

            route.data = router.data;

            route.ready();
        })
        .always(function() {
            $('#application-container').html(stache(state));

            if (window.location.hash === '#!' || !window.location.hash) {
                window.location.hash = security.isUserRequired
                                           ? '#!user/register'
                                           : '#!dashboard';
            }

            router.process();
        });
});