import {DefineMap,DefineList,Component,Reflect} from 'can';
import view from './manage.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import localisation from '~/localisation';
import Api from 'shuttle-can-api';
import $ from 'jquery';
import state from '~/state';

resources.add('message', {action: 'manage', permission: Permissions.Manage.Messages});

const Message = DefineMap.extend(
    'message',
    {
        seal: false
    },
    {
        parent: {
            type: '*'
        },
        checked: {
            type: 'boolean',
            default: false
        },

        selected: {
            type: 'boolean',
            default: false
        },

        messageSelected: function () {
            this.viewModel.messageSelected(this);
        }
    }
);

var api = {
    messages: new Api({
        endpoint: 'messages',
        Map: Message
    }),
    fetch: new Api({
        endpoint: 'messages/fetch'
    }),
    transfer: new Api({
        endpoint: 'messages/transfer'
    }),
    transferDirect: new Api({
        endpoint: 'messages/transferdirect'
    })
}

export const ViewModel = DefineMap.extend(
    'manage',
    {
        message: {
            Default: Message
        },
        messageRows: {
            Default: DefineList
        },
        messages: {
            Default: DefineList
        },
        hasMessages: {
            type: 'boolean',
            default: false
        },
        showMessages: {
            type: 'boolean',
            default: true
        },
        sourceQueueUri: {
            type: 'string',
            default: ''
        },
        destinationQueueUri: {
            type: 'string',
            default: ''
        },
        fetching: {
            type: 'boolean',
            default: false
        },
        fetchCount: {
            type: 'number',
            default: 5
        },
        refreshTimestamp: {
            type: 'string'
        },
        messageActions: {
            Default: DefineList
        },

        showTitle() {
            state.title = localisation.value('message:' + (this.showMessages ? 'title-manage' : 'message'));
        },

        get messagesPromise() {
            const self = this;
            const refreshTimestamp = this.refreshTimestamp;

            return api.messages.list()
                .then(function (list) {
                    self.messages = list;
                    self.hasMessages = list.length > 0;

                    Reflect.each(list, function (message) {
                        message.viewModel = self;
                    })

                    return list;
                });
        },

        columns: {
            Default: DefineList
        },

        messageColumns: {
            default: [
                {
                    columnClass: 'col-2',
                    columnTitle: 'name',
                    attributeName: 'name'
                },
                {
                    columnClass: 'col',
                    columnTitle: 'value',
                    attributeName: 'value'
                }
            ]
        },

        hasSourceQueueUri: {
            get() {
                return !!this.sourceQueueUri;
            }
        },

        hasDestinationQueueUri: {
            get() {
                return !!this.destinationQueueUri;
            }
        },

        noCheckedMessages: {
            get() {
                var checked = false;

                Reflect.each(this.messages, function (item) {
                    if (checked) {
                        return;
                    }

                    checked = checked || item.checked;
                });

                return !checked;
            }
        },

        init: function () {
            var self = this;
            let columns = this.columns;
            let messageActions = this.messageActions;

            if (!columns.length) {
                columns.push({
                    columnClass: 'col-1',
                    columnTitle: 'check',
                    stache: '<cs-checkbox checked:bind="checked"/>'
                });

                columns.push({
                    columnClass: 'col-1',
                    columnTitle: 'view',
                    stache: '<cs-button text:from="\'view\'" click:from="messageSelected" elementClass:from="\'btn-secondary btn-sm\'"/>'
                });

                columns.push(
                    {
                        columnClass: 'col-2',
                        columnTitle: 'message:message-id',
                        attributeName: 'messageId'
                    });

                columns.push(
                    {
                        columnClass: 'col',
                        columnTitle: 'message:message',
                        stache: '<pre>{{message}}</pre>'
                    });
            }

            if (!messageActions.length) {
                messageActions.push({
                    text: "message:return-to-source",
                    click: function () {
                        self._transfer('ReturnToSourceQueue');
                    }
                });

                messageActions.push({
                    text: "message:send-to-recipient",
                    click: function () {
                        self._transfer('SendToRecipientQueue');
                    }
                });

                messageActions.push({
                    text: "message:stop-ignoring",
                    click: function () {
                        self._transfer('StopIgnoring');
                    }
                });

                messageActions.push({
                    isSeparator: true
                });

                messageActions.push({
                    text: "move",
                    click: function () {
                        self._transfer('Move');
                    }
                });

                messageActions.push({
                    text: "copy",
                    click: function () {
                        self._transfer('Copy');
                    }
                });

                messageActions.push({
                    isSeparator: true
                });

                messageActions.push({
                    text: "remove",
                    click: function () {
                        self._transfer('Remove');
                    }
                });
            }

            this.showTitle();
            this.refresh();
        },

        refresh: function () {
            this.refreshTimestamp = Date.now();
        },

        fetch: function () {
            var self = this;

            if (!this.hasSourceQueueUri) {
                state.alerts.add({
                    message: localisation.value('message:exceptions.source-queue-uri'),
                    name: 'message:exceptions.source-queue-uri',
                    type: 'danger'
                });

                return false;
            }
            else {
                state.alerts.remove({name: 'message:exceptions.source-queue-uri'});
            }

            this.fetching = true;

            api.fetch.post({
                queueUri: this.sourceQueueUri,
                count: this.fetchCount || 1
            })
                .then(function (response) {
                    state.alerts.add({
                        message: localisation.value('message:count-retrieved', {count: response.data.countRetrieved}),
                        name: 'message:count-retrieved'
                    });

                    self.refresh();

                    self.fetching = false;
                })
                .catch(function () {
                    self.fetching = false;
                });

            return true;
        },

        checkedMessageIds: function () {
            return $.map(this.messages, function (message) {
                return message.checked ? message.messageId : undefined;
            });
        },

        move: function () {
            return this._transfer('Move');
        },

        copy: function () {
            return this._transfer('Copy');
        },

        moveDirect: function () {
            return this._transferDirect('Move');
        },

        copyDirect: function () {
            return this._transferDirect('Copy');
        },

        returnToSourceQueue: function () {
            return this._transfer('Copy');
        },

        validateDestinationQueue(action) {
            if (!this.destinationQueueUri && (action === 'Move' || action === 'Copy')) {
                state.alerts.add({
                    message: localisation.value('message:exceptions.destination-queue-uri'),
                    name: 'message:exceptions.destination-queue-uri',
                    type: 'danger'
                });

                return false;
            }

            return true;
        },

        _transfer: function (action) {
            const self = this;

            if (!this.validateDestinationQueue(action)) {
                return false;
            }

            api.transfer.post({
                messageIds: this.checkedMessageIds(),
                destinationQueueUri: this.destinationQueueUri,
                action: action
            })
                .then(function (response) {
                    self.refresh();

                    return response;
                });

            return true;
        },

        _transferDirect: function (action) {
            const self = this;

            if (!this.sourceQueueUri) {
                state.alerts.add({
                    message: localisation.value('message:exceptions.source-queue-uri'),
                    name: 'message:exceptions.source-queue-uri',
                    type: 'danger'
                });

                return false;
            }

            if (this.fetchCount < 1) {
                state.alerts.add({
                    message: localisation.value('message:exceptions.count'),
                    name: 'message:exceptions.count',
                    type: 'danger'
                });

                return false;
            }

            if (!this.validateDestinationQueue(action)) {
                return false;
            }

            api.transferDirect.post({
                sourceQueueUri: this.sourceQueueUri,
                destinationQueueUri: this.destinationQueueUri,
                action: action,
                count: this.fetchCount
            })
                .then(function () {
                    state.alerts.add({
                        message: localisation.value('message:transfer-complete-' + action.toLowerCase()),
                        name: 'message:transfer-complete'
                    });
                });

            return true;
        },

        closeMessageView() {
            this.showMessages = true;
            this.showTitle();
        },

        messageSelected: function (message) {
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
                Reflect.each(message.failureMessages, function (item, index) {
                    self.addMessageRow('FailureMessages.' + index, item);
                });
            } else {
                this.addMessageRow('FailureMessages', "(none)");
            }

            if (message.headers && message.headers.length) {
                Reflect.each(message.headers, function (item, index) {
                    self.addMessageRow('Headers.' + item.key, item.value);
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

            this.showMessages = false;
            this.showTitle();
        },

        addMessageRow: function (name, value) {
            this.messageRows.push({name: name, value: value});
        },

        checkAll: function () {
            this._setCheckMarks(true);
        },

        checkNone: function () {
            this._setCheckMarks(false);
        },

        checkInvert: function () {
            this._setCheckMarks();
        },

        _setCheckMarks: function (value) {
            Reflect.each(this.messages, function (item) {
                item.checked = (value == undefined ? !item.checked : value);
            });
        }
    });


export default Component.extend({
    tag: 'sentinel-message-manage',
    ViewModel,
    view
});