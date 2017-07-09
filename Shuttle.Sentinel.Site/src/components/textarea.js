import Component from 'can-component';
import InputViewModel from './input-view-model';
import view from './textarea.stache!';

export const ViewModel = InputViewModel.extend({
    rows: {
        type: 'number',
        value: 3
    },
    columns: {
        type: 'number',
        value: 80
    },
    value: {
        type: 'string',
        value: ''
    }
});

export default Component.extend({
    tag: 'sentinel-textarea',
    ViewModel,
    view
});


