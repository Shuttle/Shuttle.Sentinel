import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import view from './add.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import router from '~/router';
import Api from '~/api';
import validator from 'can-define-validate-validatejs';

resources.add('queue', { action: 'add', permission: Permissions.Manage.Queues });

var queues = new Api('queues/{id}');

export const ViewModel = Model.extend(
    'queues',
    {
        uri: {
            value: '',
            get: function(value) {
                var result = value;

                if (!value) {
                    result = state.get('queue');

                    if (result) {
                        result = result.attr('uri');
                    }
                }

                return result || value;
            }
        },

        add: function() {
            if (!!this.errors()) {
                return false;
            }

            queues.post({
                uri: this.attr('uri')
            });

            this.close();

            return false;
        },

        close: function() {
            state.goto('queue/list');
        }
    }
);

validator(ViewModel);

export default Component.extend({
    tag: 'sentinel-queue-add',
    viewModel: ViewModel,
    template
});