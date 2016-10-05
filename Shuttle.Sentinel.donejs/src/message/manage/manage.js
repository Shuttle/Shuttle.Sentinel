import can from 'can';
import Component from 'can/component/';
import Map from 'can/map/';
import List from 'can/list/';
import template from './manage.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import Model from 'sentinel/model';
import alerts from 'sentinel/alerts';
import api from 'sentinel/api';
import localisation from 'sentinel/localisation';
import validation from 'sentinel/validation';

resources.add('message', { action: 'manage', permission: Permissions.Manage.Users });

export const MessageModel = Map.extend({
    define: {
        checked: {
            value: false
        }
    }
});

export const ViewModel = Model.extend({
    define: {
        columns: {
            value: new List()
        },

        messages: {
            valie: new List()
        },

        sourceQueueUri: {
            value: ''
        },

        sourceQueueUriConstraint: {
            get: function() {
                return validation.get('sourceQueueUri', this.attr('sourceQueueUri'), {
                    sourceQueueUri: {
                        presence: true
                    }
                });
            }
        }
    },

    init: function() {
        var self = this;

        this.attr('columns').push(new Map(
        {
            checked: false,
            columnClass: 'col-md-1',
            columnTitleTemplate: '<sentinel-input type="checkbox" ($click)="toggleSelection()" {(checked)}="checked"/>---should be button',
            columnType: 'template',
            template: '<sentinel-input type="checkbox" {(checked)}="checked"/>',
            toggleSelection: function() {
                self.toggleSelection(!this.attr('checked'));
            }
        }));

        this.attr('columns').push(
        {
            columnTitle: 'message:message-id',
            attributeName: 'messageId'
        });

        this.refresh();
    },

    refresh: function() {
        var self = this;

        this.get('messages')
            .done(function(messages) {
                self.attr('messages', new List());

                can.each(messages, function(message) {
                    self.attr('messages').push(new MessageModel(message));
                });
            });
    },

    toggleSelection: function(value) {
        can.each(this.attr('messages'), function(item) {
            item.attr('checked', value);
        });
    },

    fetchMessage: function() {
        var self = this;

        if (!!this.attr('sourceQueueUriConstraint')) {
            return false;
        }

        this.attr('fetchingMessage', true);

        api.post('messages/fetch', {
                data: {
                    queueUri: this.attr('sourceQueueUri'),
                    count: 1
                }
            })
            .done(function(response) {
                self.refresh();
            })
            .always(function() {
                this.attr('fetchingMessage', false);
            });

        return true;
    }
});

export default Component.extend({
    tag: 'sentinel-message-manage',
    viewModel: ViewModel,
    template
});