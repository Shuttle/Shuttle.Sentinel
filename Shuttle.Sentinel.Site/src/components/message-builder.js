import Component from 'can-component';
import view from './message-builder.stache';
import validator from 'can-define-validate-validatejs';
import Api from "shuttle-can-api";
import DefineMap from 'can-define/map/';
import ComponentViewModel from 'shuttle-canstrap/infrastructure/component-view-model';


export const ViewModel = ComponentViewModel.extend(
    'message-builder',
    {
        seal: false
    },
    {

        messageType: {
            type: 'string',
            default: ''
        },

        emptyMessageType: {
            type: 'string',
            default: ''
        }
    }
);

validator(ViewModel);

export default Component.extend({
    tag: 'sentinel-message-builder',
    ViewModel,
    view
});