import Component from 'can-component';
import DefineMap from 'can-define/map/';
import template from './text.stache!';

export const ViewModel = DefineMap.extend({
    define: {
        inputClass: {
            value: ''
        },

        formGroupClass: {
            value: ''
        }
    }
});

export default Component.extend({
    tag: 'sentinel-text',
    template,
    viewModel: ViewModel
});


