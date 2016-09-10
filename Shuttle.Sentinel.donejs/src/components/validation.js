import can from 'can';
import Map from 'can/map/';
import template from './validation.stache!';

export const ViewModel = Map.extend({
    define: {
        name: {
            value: ''
        },

        classVisibility: {
            get: function() {
                return !this.attr('validationMessage') ? 'hidden' : '';
            }
        },
        
        validationMessage: {
            get: function (value) {
                return this.attr('errors.' + value) || '';
            }
        }
    }
});

export default can.Component.extend({
    tag: 'sentinel-validation',
    viewModel: ViewModel,
    template
});


