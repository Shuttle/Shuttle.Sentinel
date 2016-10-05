import can from 'can';
import Map from 'can/map/';
import template from './text.stache!';

export const ViewModel = Map.extend({
    define: {
        buttonClassType: {
            get: function(value) {
                return value || 'btn-default';
            }
        },

        buttonText: {
            get: function(value) {
                return value || '?';
            }
        }
    }
});

export default can.Component.extend({
    tag: 'sentinel-text',
    template,
    viewModel: ViewModel
});


