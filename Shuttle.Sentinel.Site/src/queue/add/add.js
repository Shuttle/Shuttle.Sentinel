import {DefineMap,Component} from 'can';
import view from './add.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import router from '~/router';
import Api from 'shuttle-can-api';
import validator from 'can-define-validate-validatejs';
import state from '~/state';
import {OptionList} from 'shuttle-canstrap/select/';

resources.add('queue', {action: 'add', permission: Permissions.Manage.Queues});

var queues = new Api({
    endpoint: 'queues/{id}'
});

export const ViewModel = DefineMap.extend(
    'queue',
    {
        init() {
            state.title = 'queue:list.title';

            const result = state.stack.pop('queue');

            if (!result) {
                return;
            }

            this.uri = result.uri;
            this.processor = result.processor;
            this.type = result.type;
        },

        uri: {
            default: '',
            validate: {
                presence: true
            }
        },

        processor: {
            type: 'string',
            default: 'unknown',
            validate: {
                presence: true
            }
        },

        processorOptions: {
            Type: OptionList,
            get() {
                return [
                    {
                        value: 'unknown',
                        label: 'unknown'
                    },
                    {
                        value: 'inbox',
                        label: 'inbox'
                    },
                    {
                        value: 'outbox',
                        label: 'outbox'
                    },
                    {
                        value: 'control-inbox',
                        label: 'control-inbox'
                    }
                ]
            }
        },

        type: {
            type: 'string',
            default: 'unknown',
            validate: {
                presence: true
            }
        },

        typeOptions: {
            Type: OptionList,
            get() {
                var result = [
                    {
                        value: 'unknown',
                        label: 'unknown'
                    },
                    {
                        value: 'work',
                        label: 'work'
                    },
                    {
                        value: 'error',
                        label: 'error'
                    }
                ];

                if (this.processor === 'inbox' || this.processor == 'unknown') {
                    result.push({
                        value: 'deferred',
                        label: 'deferred'
                    });
                } else {
                    if (this.type === 'deferred') {
                        this.type = 'unknown';
                    }
                }

                return result;
            }
        },

        add: function () {
            if (!!this.errors()) {
                return false;
            }

            queues.post({
                uri: this.uri,
                processor: this.processor,
                type: this.type
            });

            this.close();

            return false;
        },

        close: function () {
            router.goto({
                resource: 'queue',
                action: 'list'
            });
        }
    }
);

validator(ViewModel);

export default Component.extend({
    tag: 'sentinel-queue-add',
    ViewModel,
    view
});