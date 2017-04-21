import can from 'can';
import Map from 'can/map/';
import template from './checkbox.stache!';

export const ViewModel = Map.extend({
    define: {
        elementClass: {
            value: ''
        },

        checked: {
            value: false
        }
    }
});

export default can.Component.extend({
    tag: 'sentinel-checkbox',
    template,
    viewModel: ViewModel
});


