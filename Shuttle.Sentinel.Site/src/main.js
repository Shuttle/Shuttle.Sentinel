import $ from 'jquery';
import 'popper.js';
import 'bootstrap';
import 'moment';
import 'tempusdominus';
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
import '~/endpoint/';
import '~/message/';
import '~/messageheader/';
import '~/messagetypeassociation/';
import '~/messagetypedispatched/';
import '~/messagetypehandled/';
import '~/queue/';
import '~/schedule/';
import '~/subscription/';

import validate from 'validate.js';

validate.extend(validate.validators.datetime, {
    // The value is guaranteed not to be null or undefined but otherwise it
    // could be anything.
    parse: function(value, options) {
        return +moment.utc(value);
    },
    // Input is a unix timestamp
    format: function(value, options) {
        var format = options.dateOnly ? "YYYY-MM-DD" : "YYYY-MM-DD hh:mm:ss";
        return moment.utc(value).format(format);
    }
});

canstrap.button.remove.confirmation = function (options) {
    state.modal.confirmation.show(options);
}

$.ajaxPrefilter(function (options, originalOptions) {
    options.error = function (xhr) {
        if (xhr.status != 200) {
            if (xhr.responseJSON) {
                state.alerts.add({message: xhr.responseJSON.message, type: 'danger', name: 'ajax-prefilter-error'});
            } else {
                state.alerts.add({
                    message: xhr.status + ' / ' + xhr.statusText,
                    type: 'danger',
                    name: 'ajax-prefilter-error'
                });
            }
        }

        if (originalOptions.error) {
            originalOptions.error(xhr);
        }
    };
});

localisation.start(function(error) {
    if (error) {
        throw new Error(error);
    }

    access.start()
        .then(function () {
            router.start();

            $('#application-container').html(stache(state));

            var sidebarCollapse = $('#sidebar-collapse')

            sidebarCollapse.on('hidden.bs.collapse', function () {
                state.sidebarCollapsed = true;
            });

            sidebarCollapse.on('shown.bs.collapse', function () {
                state.sidebarCollapsed = false;
            });

            if (window.location.hash === '#!' || !window.location.hash) {
                router.goto({resource: 'dashboard'});
            }

            router.process();
        });
});

