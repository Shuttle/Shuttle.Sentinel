import Component from 'can-component';
import ComponentViewModel from 'shuttle-canstrap/infrastructure/component-view-model';
import view from './message-builder.stache';
import validator from 'can-define-validate-validatejs';

export const ViewModel = ComponentViewModel.extend(
    'message-builder',
    {
        seal: false
    },
    {
        messageType: {
            type: 'string',
            value: '',
            validate: {
                presence: true
            }
        },
        message: {
            type: 'string',
            value: '',
            validate: {
                presence: true
            }
        }
    }
);

validator(ViewModel);

export default Component.extend({
    tag: 'sentinel-message-builder',
    ViewModel,
    view
});