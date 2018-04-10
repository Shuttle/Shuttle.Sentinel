import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import resources from '~/resources';
import Permissions from '~/permissions';
import view from './item.stache!';
import Api from 'shuttle-can-api';
import validator from 'can-define-validate-validatejs';
import guard from 'shuttle-guard';

resources.add('messageheader', {action: 'add', permission: Permissions.Manage.Messages});

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

        this.working = true;

api.messageHeaders.post({
    key: this.headerKey,
    value})
    }
});

validator(ViewModel);

export default Component.extend({
    tag: 'sentinel-message-headers',
    ViewModel,
    view
});