import Component from 'can-component';
import DefineMap from 'can-define/map/';
import template from './checkbox.stache!';

export const ViewModel = DefineMap.extend({
    define: {
        elementClass: {
            value: ''
        },

        checked: {
            value: false
        }
    }
});

export default Component.extend({
    tag: 'sentinel-checkbox',
    template,
    viewModel: ViewModel
});


