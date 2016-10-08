import can from 'can';
import Map from 'can/map/';
import template from './text.stache!';

export const ViewModel = Map.extend({
    define: {
        inputClass: {
            value: ''
        },

        formGroupClass: {
            value: ''
        }
    }
});

export default can.Component.extend({
    tag: 'sentinel-text',
    template,
    viewModel: ViewModel
});


