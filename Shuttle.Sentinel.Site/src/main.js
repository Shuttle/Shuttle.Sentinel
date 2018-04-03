import $ from 'jquery';
import 'popper.js';
import 'bootstrap';
import 'can-stache-route-helpers';

import 'bootstrap/dist/css/bootstrap.css';
import 'font-awesome/css/font-awesome.css';
import './styles.css!';

import {options as apiOptions} from 'shuttle-can-api';
import loader from '@loader';

apiOptions.url = loader.serviceBaseURL;

import stache from '~/main.stache!';
import localisation from '~/localisation';
import state from '~/state';
import router from '~/router';

import canstrap from 'shuttle-canstrap';
import access from 'shuttle-access';

access.url = loader.accessBaseURL;

import '~/components/queue-input';
import '~/components/queue';
import '~/login/';
import '~/navigation/';
import '~/dashboard/';
import '~/datastore/';
import '~/message/';
import '~/queue/';
import '~/subscription/';

canstrap.button.remove.confirmation = function (options) {
    state.modal.confirmation.show(options);
}

localisation.start(function(error) {
    if (error) {
        throw new Error(error);
    }

    access.start()
        .then(function () {
            router.start();

            $('#application-container').html(stache(state));

            if (window.location.hash === '#!' || !window.location.hash) {
                router.goto({resource: 'dashboard'});
            }

            router.process();
        });
});
