import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import view from './headers.stache!';
import Api from 'shuttle-can-api';
import validator from 'can-define-validate-validatejs';

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

    init() {
        const columns = this.columns;

        if (!columns.length) {
            columns.push({
                columnTitle: 'key',
                columnClass: 'col-4',
                attributeName: 'key'
            });

            columns.push({
                columnTitle: 'value',
                columnClass: 'col-4',
                attributeName: 'value'
            });

            columns.push({
                columnTitle: 'remove',
                columnClass: 'col-1',
                stache: '<cs-button-remove click:from="@remove" elementClass:from="\'btn-sm\'"/>'
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

    add(){
        const self = this;

        if (!!this.errors()) {
            return false;
        }

        this.headers.push({
            key: this.headerKey,
            value: this.headerValue
        });
    }
});

validator(ViewModel);

export default Component.extend({
    tag: 'sentinel-message-send-headers',
    ViewModel,
    view
});