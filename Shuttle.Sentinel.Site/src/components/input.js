import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './input.stache!';

export const ViewModel = DefineMap.extend({
    type: {
        type: 'string',
        get: function(type) {
            return type || 'text';
        }
    },

    placeholder: 'string',
    elementClass: 'string',

    aaa: { type: 'string', value: '' }
});

export default Component.extend({
    tag: 'sentinel-input',
    view,
    viewModel: ViewModel,
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


