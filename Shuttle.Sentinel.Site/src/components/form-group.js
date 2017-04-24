import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './form-group.stache!';

export const ViewModel = DefineMap.extend({
    validationMessage: 'string',
    elementClass: 'string'
    //elementClass: {
    //    get: function() {
    //        var errors = this.errors;

    //        return !!errors && !!errors.attr(this.validationName) ? 'has-error' : '';
    //    }
    //}
});

export default Component.extend({
    tag: 'sentinel-form-group',
    //viewModel: function(attrs, scope) {
    //    let map = new ViewModel(attrs);

    //    map.attr('errors', scope.errors);

    //    return map;
    //},
    viewModel: ViewModel,
    view
});


