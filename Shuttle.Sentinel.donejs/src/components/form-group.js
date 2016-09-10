import can from 'can';
import Map from 'can/map/';
import template from './form-group.stache!';

export const ViewModel = Map.extend({
    define: {
        validationName: {
            value: ''
        },

        classType: {
            get: function() {
                return !!this.attr('errors.' + this.attr('validationName')) ? 'has-error' : '';
            }
        }
    }
});

export default can.Component.extend({
    tag: 'sentinel-form-group',
    viewModel: ViewModel,
	template
});


