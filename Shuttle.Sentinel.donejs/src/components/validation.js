import can from 'can';
import Map from 'can/map/';
import template from './validation.stache!';

export const ViewModel = Map.extend({
    define: {
        message: {
            value: ''
        },

        classVisibility: {
            get: function() {
                return !this.attr('message') ? 'hidden' : '';
            }
        }
    }
});

export default can.Component.extend({
    tag: 'sentinel-validation',
    viewModel: ViewModel,
    template
});


