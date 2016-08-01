import can from 'can';
import Map from 'can/map/';
import template from './input.stache!';

export const ViewModel = Map.extend({
    define: {
        type: {
            get: function(type) {
                return type || 'text';
            }
        }
    }
});

export default can.Component.extend({
    tag: 'sentinel-input',
    template,
    viewModel: ViewModel,
    events: {
        'inserted': function(el) {
            if (this.viewModel.attr('focus')) {
                el[0].childNodes[0].focus();
            }
        }
    }
});


