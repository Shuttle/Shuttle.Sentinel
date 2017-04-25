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
    placeholder: 'string',
    elementClass: 'string',

    inputValue: { type: 'string', value: 'xyz' },
    value: { type: 'string', value: 'xyz' }
});

export default Component.extend({
    tag: 'sentinel-input',
    view,
    ViewModel: ViewModel,
    events: {
        'inserted': function(el) {
            if (this.viewModel.focus) {
                if (el[0] && el[0].childNodes[0] && el[0].childNodes[0].focus) {
                    el[0].childNodes[0].focus();
                }
            }
        }
    }
});


