import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import resources from '~/resources';
import Permissions from '~/permissions';
import view from './headers.stache!';
import Api from 'shuttle-can-api';
import validator from 'can-define-validate-validatejs';
import guard from 'shuttle-guard';

resources.add('message', {action: 'headers', permission: Permissions.Manage.Messages});

export const MessageHeaderMap = DefineMap.extend({
    key: {
        type: 'string',
        default: ''
    },
    value: {
        type: 'string',
        default: ''
    }
});

export const MessageHeaderList = DefineList.extend({
    '#': MessageHeaderMap
});

var api = {
    messageHeaders: new Api({
        endpoint: 'messageheaders/{id}',
        Map: MessageHeaderMap
    })
}

export const ViewModel = DefineMap.extend({
    columns: {
        Default: DefineList
    },

    edit(header) {
      alert(header.key);
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
                columnClass: 'col',
                attributeName: 'value'
            });

            columns.push({
                columnTitle: 'actions',
                columnClass: 'col-1',
                stache: '<cs-button click:from="scope.root.edit" text:from="\'edit\'" elementClass:from="\'btn-sm\'"/><cs-button-remove click:from="remove" elementClass:from="\'btn-sm\'"/>'
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

        var result = this.headers.filter(function(header){
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

        if (!!header){
            header.value = this.headerValue;
        }
        else {
            this.headers.push({
                key: this.headerKey,
                value: this.headerValue
            });
        }
    }
});

validator(ViewModel);

export default Component.extend({
    tag: 'sentinel-message-headers',
    ViewModel,
    view
});