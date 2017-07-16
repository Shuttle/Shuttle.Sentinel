import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './checkbox.stache!';

export const ViewModel = DefineMap.extend({
    elementClass: {
        value: ''
    },

    checked: {
        value: false
    }
});

export default Component.extend({
    tag: 'sentinel-checkbox',
    ViewModel,
    view
});


