import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './text.stache!';

export const ViewModel = DefineMap.extend('SentinelTextModel', {
    value: { type: 'string', value: '' },
    inputClass: { type: 'string', value: '' },
    formGroupClass: { type: 'string', value: '' }
});

export default Component.extend({
    tag: 'sentinel-text',
    view,
    ViewModel
});


