import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './form-group.stache!';

export const ViewModel = DefineMap.extend({
    validationMessage: { type: 'string', value: '' },
    elementClass: { type: 'string', value: '' }
});

export default Component.extend({
    tag: 'sentinel-form-group',
    ViewModel,
    view
});


