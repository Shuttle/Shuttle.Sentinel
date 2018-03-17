import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import guard from 'shuttle-guard';
import route from 'can-route';
import {alerts} from 'shuttle-canstrap/alerts/';
import loader from '@loader';
import stache from 'can-stache';
import localisation from '~/localisation';
import navbar from '~/navbar';
import stack from '~/stack';

var State = DefineMap.extend({
    route: route,
    alerts: {
        get() {
            return alerts;
        }
    },
    navbar: {
        get() {
            return navbar;
        }
    },
    stack: {
        get() {
            return stack;
        }
    },
    debug: {
        type: 'boolean',
        get() {
            return loader.debug;
        }
    },
    title: {
        type: 'string',
        default: '',
        get(value) {
            return localisation.value(value);
        }
    },
    modal: {
        Type: DefineMap.extend({
            confirmation: {
                Type: DefineMap.extend({
                    primaryClick: {
                        type: '*'
                    },
                    message: {
                        type: 'string',
                        default: ''
                    },
                    show(options) {
                        guard.againstUndefined(options, "options");

                        this.message = options.message || 'No \'message\' passed in the confirmation options.';
                        this.primaryClick = options.primaryClick;

                        $('#modal-confirmation').modal({show: true});
                    }
                }),
                default() {
                    return {};
                }
            }
        }),
        default() {
            return {};
        }
    },
});

export default new State();
