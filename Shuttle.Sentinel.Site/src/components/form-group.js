import can from 'can';
import Map from 'can/map/';
import template from './form-group.stache!';

export const ViewModel = Map.extend({
    define: {
        validationMessage: {
            value: ''
        },

        elementClass: {
            get: function() {
                var errors = this.attr('errors');

                return !!errors && !!errors.attr(this.attr('validationName')) ? 'has-error' : '';
            }
        }
    }
});

export default can.Component.extend({
    tag: 'sentinel-form-group',
    viewModel: function(attrs, scope) {
        let map = new ViewModel(attrs);

        map.attr('errors', scope.attr('errors'));

        return map;
    },
	template
});


