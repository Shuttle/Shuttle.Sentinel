import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './label.stache!';
import localisation from '~/localisation';

export const ViewModel = DefineMap.extend({
    label: {
        get: function(value) {
            return localisation.value(value);
        }
    }
});

export default Component.extend({
    tag: 'sentinel-label',
    view,
    viewModel: ViewModel
});


