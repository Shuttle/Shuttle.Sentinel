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
        },

        selected: {
            value: false
        },

        rowClass: {
            get: function() {
                return this.attr('selected') ? 'text-success success' : '';
            }
        }
    },

    toggleCheck: function(ev) {
        ev.stopPropagation();
    },

    messageSelected: function(message) {
        this.attr('viewModel').messageSelected(message);
    }
});

export const ViewModel = Model.extend({
    define: {
        columns: {
            value: new List()
        },

        messages: {
            value: new List()
        },

        messageColumns: {
            value: [
                {
                    columnClass: 'col-md-2',
                    columnTitle: 'name',
                    attributeName: 'name'
                },
                {
                    columnTitle: 'value',
                    attributeName: 'value'
                }
            ]
        },

        messageActions: {
            value: new List()
        },

        checkActions: {
            value: new List()
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
        let columns = this.attr('columns');
        let messageActions = this.attr('messageActions');
        let checkActions = this.attr('checkActions');

        if (!columns.length) {
            columns.push(new Map(
            {
                checked: false,
                columnClass: 'col-md-1',
                columnType: 'template',
                template: '<sentinel-input type="checkbox" {(checked)}="checked" ($click)="toggleCheck(%event)"/>'
            }));

            columns.push(
            {
                columnTitle: 'message:message-id',
                attributeName: 'messageId'
            });
        }

        if (!messageActions.length) {
            messageActions.push({
                text: "message:return-to-source",
                click: function() {
                    alert('return');
                }
            });

            messageActions.push({
                text: "message:send-to-recipient",
                click: function() {
                    alert('recipient');
                }
            });

            messageActions.push({
                text: "message:stop-ignoring",
                click: function() {
                    alert('stop ignoring');
                }
            });

            messageActions.push({
                text: "remove",
                click: function() {
                    alert('remove');
                }
            });
        }

        if (!checkActions.length) {
            checkActions.push({
                text: "all",
                click: function() {
                    self.checkAll();
                }
            });

            checkActions.push({
                text: "none",
                click: function() {
                    self.checkNone();
                }
            });

            checkActions.push({
                text: "invert",
                click: function() {
                    self.checkInvert();
                }
            });
        }
        this.refresh();
    },

    refresh: function() {
        var self = this;

        this.get('messages')
            .done(function(messages) {
                self.attr('messages', new List());

                can.each(messages, function(message) {
                    let messageModel = new MessageModel(message);

                    messageModel.attr('viewModel', self);

                    self.attr('messages').push(messageModel);
                });
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
    },

    messageSelected: function(message) {
        if (this.attr('message')) {
            this.attr('message.selected', false);
        }

        this.attr('message', message);
        this.attr('message.selected', true);
        this.attr('messageRows', new List());

        this.addMessageRow('MessageId', message.attr('messageId'));
        this.addMessageRow('SourceQueueUri', message.attr('sourceQueueUri'));
        this.addMessageRow('AssemblyQualifiedName', message.attr('assemblyQualifiedName'));
        this.addMessageRow('CompressionAlgorithm', message.attr('compressionAlgorithm'));
        this.addMessageRow('CorrelationId', message.attr('correlationId'));
        this.addMessageRow('EncryptionAlgorithm', message.attr('encryptionAlgorithm'));
        this.addMessageRow('ExpiryDate', message.attr('expiryDate'));

        if (message.attr('failureMessages') && message.attr('failureMessages').length) {
            can.each(message.attr('failureMessages'), function(item, index) {
                this.addMessageRow('FailureMessages.' + index, item);
            });
        } else {
            this.addMessageRow('FailureMessages', "(none)");
        }

        if (message.attr('headers') && message.attr('headers').length) {
            can.each(message.attr('headers'), function(item, index) {
                this.addMessageRow('Headers.' + index, item);
            });
        } else {
            this.addMessageRow('Headers', "(none)");
        }

        this.addMessageRow('IgnoreTillDate', message.attr('ignoreTillDate'));
        this.addMessageRow('MessageReceivedId', message.attr('messageReceivedId'));
        this.addMessageRow('MessageType', message.attr('messageType'));
        this.addMessageRow('PrincipalIdentityName', message.attr('principalIdentityName'));
        this.addMessageRow('RecipientInboxWorkQueueUri', message.attr('recipientInboxWorkQueueUri'));
        this.addMessageRow('SendDate', message.attr('sendDate'));
        this.addMessageRow('SenderInboxWorkQueueUri', message.attr('senderInboxWorkQueueUri'));
    },

    addMessageRow: function(name, value) {
        this.attr('messageRows').push({ name: name, value: value });
    },

    checkAll: function() {
        this._setCheckMarks(true);
    },

    checkNone: function() {
        this._setCheckMarks(false);
    },

    checkInvert: function() {
        this._setCheckMarks();
    },

    _setCheckMarks: function(value) {
        can.each(this.attr('messages'), function(item) {
            item.attr('checked', value == undefined? !item.attr('checked'): value);
        });
    }
});

export default Component.extend({
    tag: 'sentinel-message-manage',
    viewModel: ViewModel,
    template
});