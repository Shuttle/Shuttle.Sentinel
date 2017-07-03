import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import view from './manage.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import router from '~/router';
import alerts from '~/alerts';
import localisation from '~/localisation';
import api from '~/api';
import each from 'can-util/js/each/';
import $ from 'jquery';
import Message from '~/models/message';

resources.add('message', { action: 'manage', permission: Permissions.Manage.Users });

export const ViewModel = DefineMap.extend({
    fetching: {
        type: 'boolean',
        value: false
    },

    fetchCount: {
        type: 'number',
        value: 5
    },

    refreshTimestamp: {
        type: 'string'
    },

    get messagesPromise() {
        const refreshTimestamp = this.refreshTimestamp;
        return Message.getList({});
    },

    columns: {
        value: new DefineList()
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
        value: new DefineList()
    },

    sourceQueueUri: {
        value: ''
    },

    hasMessages: {
        get: function() {
            return this.messages.length > 0;
        }
    },

    get messages() {
        return this.messagesPromise.isResolved ? this.messagesPromise.value : new DefineList();
    },

    hasSourceQueueUri: {
        get: function() {
            return !!this.sourceQueueUri;
        }
    },

    hasDestinationQueueUri: {
        get: function() {
            return !!this.destinationQueueUri;
        }
    },

    noCheckedMessages: {
        get: function() {
            var checked = false;

            each(this.messages, function(item) {
                if (checked) {
                    return;
                }

                checked = checked || item.checked;
            });

            return !checked;
        }
    },

    init: function() {
        var self = this;
        let columns = this.columns;
        let messageActions = this.messageActions;

        if (!columns.length) {
            columns.push({
                checked: false,
                columnClass: 'col-md-1',
                columnTitle: 'check',
                columnType: 'template',
                template: '<span ($click)="toggleCheck(%event)" class="glyphicon {{#if checked}}glyphicon-check{{else}}glyphicon-unchecked{{/if}}" />'
            });

            columns.push(
            {
                columnClass: 'col-md-2',
                columnTitle: 'message:message-id',
                attributeName: 'messageId'
            });

            columns.push(
            {
                columnTitle: 'message:message',
                columnType: 'template',
                template: '<pre>{{message}}</pre>'
            });
        }

        if (!messageActions.length) {
            messageActions.push({
                text: "message:return-to-source",
                click: function() {
                    self._move('ReturnToSourceQueue');
                }
            });

            messageActions.push({
                text: "message:send-to-recipient",
                click: function() {
                    self._move('SendToRecipientQueue');
                }
            });

            messageActions.push({
                text: "message:stop-ignoring",
                click: function() {
                    self._move('StopIgnoring');
                }
            });

            messageActions.push({
                text: "remove",
                click: function() {
                    self._move('Remove');
                }
            });
        }

        this.refresh();
    },

    showMessages: function() {
        this.message = undefined;
    },

    refresh: function() {
        this.refreshTimestamp = Date.now();
    },

    fetch: function() {
        var self = this;

        if (!this.hasSourceQueueUri) {
            alerts.show({ message: localisation.value('message:exceptions.source-queue-uri'), name: 'message:exceptions.source-queue-uri', type: 'danger' });

            return false;
        }

        this.fetching = true;

        api.post('messages/fetch', {
            data: {
                queueUri: this.sourceQueueUri,
                count: this.fetchCount || 1
            }
        })
            .done(function(response) {
                alerts.show({ message: localisation.value('message:count-retrieved', { count: response.data.countRetrieved }), name: 'message:count-retrieved'});

                self.refresh();
            })
            .always(function() {
                self.fetching = false;
            });

        return true;
    },

    checkedMessageIds: function() {
        return $.map(this.messages, function(message) { return message.checked ? message.messageId : undefined; });
    },

    move: function() {
        return this._move('Move');
    },

    copy: function() {
        return this._move('Copy');
    },

    returnToSourceQueue: function() {
        return this._move('Copy');
    },

    _move: function(action) {
        var self = this;

        if (!this.destinationQueueUri && (action === 'Move' || action === 'Copy')) {
            alerts.show({ message: localisation.value('message:exceptions.destination-queue-uri'), name: 'message:exceptions.destination-queue-uri', type: 'danger' });

            return false;
        }

        this.moving = true;

        api.post('messages/move', {
            data: {
                messageIds: this.checkedMessageIds(),
                destinationQueueUri: this.destinationQueueUri,
                action: action
            }
        })
            .done(function() {
                self.refresh();
            })
            .always(function() {
                self.moving = false;
            });

        return true;
    },

    messageSelected: function(message) {
        var self = this;

        this.message = message;
        this.messageRows = new DefineList();

        this.addMessageRow('MessageId', message.messageId);
        this.addMessageRow('SourceQueueUri', message.sourceQueueUri);
        this.addMessageRow('AssemblyQualifiedName', message.assemblyQualifiedName);
        this.addMessageRow('CompressionAlgorithm', message.compressionAlgorithm);
        this.addMessageRow('CorrelationId', message.correlationId);
        this.addMessageRow('EncryptionAlgorithm', message.encryptionAlgorithm);
        this.addMessageRow('ExpiryDate', message.expiryDate);

        if (message.failureMessages && message.failureMessages.length) {
            each(message.failureMessages, function(item, index) {
                self.addMessageRow('FailureMessages.' + index, item);
            });
        } else {
            this.addMessageRow('FailureMessages', "(none)");
        }

        if (message.headers && message.headers.length) {
            each(message.headers, function(item, index) {
                self.addMessageRow('Headers.' + index, item);
            });
        } else {
            this.addMessageRow('Headers', "(none)");
        }

        this.addMessageRow('IgnoreTillDate', message.ignoreTillDate);
        this.addMessageRow('MessageReceivedId', message.messageReceivedId);
        this.addMessageRow('MessageType', message.messageType);
        this.addMessageRow('PrincipalIdentityName', message.principalIdentityName);
        this.addMessageRow('RecipientInboxWorkQueueUri', message.recipientInboxWorkQueueUri);
        this.addMessageRow('SendDate', message.sendDate);
        this.addMessageRow('SenderInboxWorkQueueUri', message.senderInboxWorkQueueUri);
    },

    addMessageRow: function(name, value) {
        this.messageRows.push({ name: name, value: value });
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
        each(this.messages, function(item) {
            item.checked = (value == undefined? !item.checked: value);
        });
    }
});

export default Component.extend({
    tag: 'sentinel-message-manage',
    ViewModel,
    view
});