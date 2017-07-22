import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './select.stache!';

export const ViewModel = DefineMap.extend({
    options: {},
    value: { type: 'string', value: '' },

    elementClass: {
        value: ''
    }
});

export default Component.extend({
    tag: 'sentinel-select',
    ViewModel,
    view
});


