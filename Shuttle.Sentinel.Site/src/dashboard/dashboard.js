import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import view from './dashboard.stache!';
import resources from '~/resources';
import localisation from '~/localisation';
import Permissions from '~/permissions';
import state from '~/state';
import Api from 'shuttle-can-api';
import {Map} from "../endpoint/list/list";

localisation.addNamespace('dashboard');

resources.add('dashboard', { permission: Permissions.View.Dashboard });

const MetricMap = DefineMap.extend({
    messageType: {
        type: 'string'
    },
    count: {
        type: 'number'
    },
    totalExecutionDuration: {
        type: 'number'
    },
    fastestExecutionDuration: {
        type: 'number'
    },
    slowestExecutionDuration: {
        type: 'number'
    },
    averageExecutionDuration: {
        type: 'number'
    }
});

var api = {
    endpoints: new Api({
        endpoint: 'endpoints/statistics',
        Map: DefineMap.extend({
            upCount: {
                type: 'number'
            },
            downCount: {
                type: 'number'
            },
            recoveryCount: {
                type: 'number'
            }
        })
    }),
    metrics: new Api({
        endpoint: 'messagetypemetrics/search',
        Map: MetricMap
    })
};

const EndpointsViewModel = DefineMap.extend({
    working: {
        type: 'boolean',
        default: false
    },

    upCount: {
        type: 'number'
    },

    downCount: {
        type: 'number'
    },

    recoveryCount: {
        type: 'number'
    },

    interval: {
        type: 'number',
        default: 5
    },

    refreshDate: {
        type: 'date',
        default: new Date()
    },

    refresh(){
        var self = this;

        if (new Date() < this.refreshDate || this.working){
            return;
        }

        this.working = true;

        api.endpoints.map()
            .then(function(map){
                self.upCount = map.upCount;
                self.downCount = map.downCount;
                self.recoveryCount = map.recoveryCount;
            })
            .then(function(){
                var t = new Date();

                t.setSeconds(t.getSeconds() + self.interval);

                self.refreshDate = t;

                self.working = false;
            });
    }
});

const MetricsViewModel = DefineMap.extend({
    working: {
        type: 'boolean',
        default: false
    },

    interval: {
        type: 'number',
        default: 5
    },

    refreshDate: {
        type: 'date',
        default: new Date()
    },

    columns: {
        Default: DefineList
    },

    refreshTimestamp: {type: 'string'},

    get listPromise() {
        const self = this;
        const refreshTimestamp = this.refreshTimestamp;

        var t = new Date();

        t.setMinutes(t.getMinutes() - 5);

        return api.metrics.post({ from: t.toISOString() })
            .then(function(){
                var t = new Date();

                t.setSeconds(t.getSeconds() + self.interval);

                self.refreshDate = t;

                self.working = false;
            });
    },

    init: function () {
        const columns = this.columns;

        if (!columns.length) {
            columns.push({
                columnTitle: 'endpoint:message-type',
                columnClass: 'col-2',
                attributeName: 'messageType'
            });

            columns.push({
                columnTitle: 'count',
                columnClass: 'col-2',
                attributeName: 'count'
            });

            columns.push({
                columnTitle: 'duration',
                columnClass: 'col-2',
                attributeName: 'totalExecutionDuration'
            });

            columns.push({
                columnTitle: 'average',
                columnClass: 'col-2',
                attributeName: 'everageExecutionDuration'
            });

            columns.push({
                columnTitle: 'fastest',
                columnClass: 'col-2',
                attributeName: 'fastestExecutionDuration'
            });

            columns.push({
                columnTitle: 'slowest',
                columnClass: 'col-2',
                attributeName: 'slowestExecutionDuration'
            });

        }
    },

    refresh: function () {
        var self = this;

        if (new Date() < this.refreshDate || this.working){
            return;
        }

        this.working = true;

        this.refreshTimestamp = Date.now();
    }
});

export const ViewModel = DefineMap.extend({
    init () {
        state.title = 'dashboard:title';

        this.refresh();
    },

    endpoints: {
        Default: EndpointsViewModel
    },

    metrics: {
        Default: MetricsViewModel
    },

    refresh(){
        const self = this;

        this.endpoints.refresh();
        this.metrics.refresh();

        setTimeout(function(){
            self.refresh.call(self);
        }, 1000);
    }
});

export default Component.extend({
    tag: 'sentinel-dashboard',
    view: view,
    viewModel: ViewModel
});

