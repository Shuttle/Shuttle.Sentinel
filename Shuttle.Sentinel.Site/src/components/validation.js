import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './validation.stache!';

export const ViewModel = DefineMap.extend({
    define: {
        message: {
            value: ''
        },

        classVisibility: {
            get: function() {
                return !this.message ? 'hidden' : '';
            }
        }
    }
});

export default Component.extend({
    tag: 'sentinel-validation',
    viewModel: ViewModel,
    view
});


