import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './input.stache!';

export const ViewModel = DefineMap.extend(
    'sentinel-input-model',
    {
        type: {
            type: 'string',
            get: function(type) {
                return type || 'text';
            }
        },

        checked: 'boolean',
        placeholder: { type: 'string', value: '' },
        elementClass: { type: 'string', value: '' },
        focus: { type: 'boolean', value: true },

        value: { type: 'string', value: '' }
    });

export default Component.extend({
    tag: 'sentinel-input',
    view,
    ViewModel
});