import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import guard from 'shuttle-guard';
import route from 'can-route';
import {alerts} from 'shuttle-canstrap/alerts/';
import loader from '@loader';
import localisation from '~/localisation';
import navbar from '~/navbar';
import stack from '~/stack';
import access from "shuttle-access";
import map from "./navigation/navigation-map";
import each from 'can-util/js/each/';

var State = DefineMap.extend({
    sidebarCollapsed: {
        type: 'boolean'
    },
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
            return !!value ? localisation.value(value) : this.sidebarCollapsed ? 'Shuttle.Sentinel' : '';
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
    resources: {
        get: function (value) {
            var result = new DefineList();

            each(map, function (item) {
                var add = false;
                var list = new DefineList();

                if (!item.permission || access.hasPermission(item.permission)) {
                    if (item.items !== undefined) {
                        each(item.items, function (subitem) {
                            if (!subitem.permission || access.hasPermission(subitem.permission)) {
                                add = true;

                                list.push(new DefineMap({
                                    href: subitem.href,
                                    text: subitem.text
                                }));
                            }
                        });
                    } else {
                        add = true;
                    }

                    if (add) {
                        result.push({
                            text: item.text,
                            href: item.href || '',
                            list: list
                        });
                    }
                }
            });

            return result;
        }
    }
});

export default new State();
