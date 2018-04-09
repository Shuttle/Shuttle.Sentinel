import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import view from './headers.stache!';
import Api from 'shuttle-can-api';
import validator from 'can-define-validate-validatejs';
import guard from 'shuttle-guard';

export const MessageHeaderMap = DefineMap.extend({
    key: {
        type: 'string',
        default: ''
    },
    value: {
        type: 'string',
        default: ''
    },
    saved: {
        type: 'boolean',
        default: false
    },
    edit() {
        this.viewModel.edit(this);
    },
    remove() {
        this.viewModel.remove(this);
    },
    viewModel: {
        type: '*'
    }
});

export const MessageHeaderList = DefineList.extend({
    '#': MessageHeaderMap
});

var api = {
    messageHeaders: new Api({
        endpoint: 'messageheaders/{search}',
        Map: MessageHeaderMap
    })
}

export const ViewModel = DefineMap.extend({
    columns: {
        Default: DefineList
    },

    edit(header) {
        guard.againstUndefined(header, 'header');

        this.headerKey = header.key || '';
        this.headerValue = header.value || '';
    },

    remove(header) {
        guard.againstUndefined(header, 'header');

        var match = header.key.toLowerCase();

        this.headers = this.headers.filter(function (header) {
            return header.key.toLowerCase() !== match;
        });
    },

    init() {
        const columns = this.columns;

        if (!columns.length) {
            columns.push({
                columnTitle: 'key',
                columnClass: 'col-5',
                attributeName: 'key'
            });

            columns.push({
                columnTitle: 'value',
                columnClass: 'col-5',
                attributeName: 'value'
            });

            columns.push({
                columnTitle: 'saved',
                columnClass: 'col-1',
                stache: '<cs-checkbox click:from="toggle" checked:bind="saved" checkedClass:from="\'fa-toggle-on\'" uncheckedClass:from="\'fa-toggle-off\'"/>{{#if working}}<i class="fa fa-hourglass-o" aria-hidden="true"></i>{{/if}}'
            });

            columns.push({
                columnTitle: 'actions',
                columnClass: 'col-1',
                stache: '<cs-button click:from="edit" iconNameClass:from="\'fa-pencil\'" elementClass:from="\'btn-sm\'"/><cs-button click:from="remove" iconNameClass:from="\'fa-times\'"  elementClass:from="\'btn-sm\'"/>'
            });
        }
    },

    headerKey: {
        type: 'string',
        default: '',
        validate: {
            presence: true
        }
    },

    headerValue: {
        type: 'string',
        default: '',
        validate: {
            presence: true
        }
    },

    headers: {
        Default: MessageHeaderList
    },

    find(key) {
        guard.againstUndefined(key, 'key');

        var match = key.toLowerCase();

        var result = this.headers.filter(function (header) {
            return header.key.toLowerCase() === match;
        });

        return result.length > 0 ? result[0] : null;
    },

    add() {
        const self = this;

        if (!!this.errors()) {
            return false;
        }

        const header = this.find(this.headerKey);

        if (!!header) {
            header.value = this.headerValue;
        }
        else {
            this.headers.push({
                key: this.headerKey,
                value: this.headerValue,
                viewModel: this
            });
        }
    }
});

validator(ViewModel);

export default Component.extend({
    tag: 'sentinel-message-send-headers',
    ViewModel,
    view
});