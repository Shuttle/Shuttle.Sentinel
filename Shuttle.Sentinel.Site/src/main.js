import $ from 'jquery';
import stache from '~/main.stache!';
import localisation from '~/localisation';
import security from '~/security';
import state from '~/state';
import router from '~/router';
import route from 'can-route';

import 'bootstrap/dist/js/bootstrap'

import '~/components/alerts';
import '~/components/button';
import '~/components/buttons';
import '~/components/fetching';
import '~/components/form';
import '~/components/form-group';
import '~/components/input';
import '~/components/label';
import '~/components/modal';
import '~/components/navigation';
import '~/components/page-title';
import '~/components/refresh-button';
import '~/components/remove-button';
import '~/components/submit-button';
import '~/components/table';
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
