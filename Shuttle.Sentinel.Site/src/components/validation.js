import Component from 'can-component';
import DefineMap from 'can-define/map/';
import template from './validation.stache!';

export const ViewModel = DefineMap.extend({
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

export default Component.extend({
    tag: 'sentinel-validation',
    viewModel: ViewModel,
    template
});


