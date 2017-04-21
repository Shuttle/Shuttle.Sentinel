import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './form-group.stache!';

export const ViewModel = DefineMap.extend({
        validationMessage: {
            value: ''
        },

        elementClass: {
            get: function() {
                var errors = this.attr('errors');

                return !!errors && !!errors.attr(this.attr('validationName')) ? 'has-error' : '';
            }
        }
});

export default Component.extend({
    tag: 'sentinel-form-group',
    //viewModel: function(attrs, scope) {
    //    let map = new ViewModel(attrs);

    //    map.attr('errors', scope.attr('errors'));

    //    return map;
    //},
    viewModel: ViewModel,
	view
});


