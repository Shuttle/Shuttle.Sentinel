import Component from 'can-component';
import view from './back-button.stache!';
import DefineMap from 'can-define/map/';

export const ViewModel = DefineMap.extend({
    elementClass: {
        type: 'string',
        get: function(type) {
            return type || '';
        }
    },
    back: function() {
        window.history.back();
    }
});

export default Component.extend({
    tag: 'sentinel-back-button',
    view,
    ViewModel
});