import Component from 'can-component';
import DefineMap from 'can-define/map/';
import template from './fetching.stache!';
import localisation from '~/localisation';

export const ViewModel = DefineMap.extend({
    define: {
        title: {
            get: function(value) {
                return localisation.value('fetching', { name: localisation.value(value)});
            }
        }
    }
});

export default Component.extend({
    tag: 'sentinel-fetching',
    template,
    viewModel: ViewModel
});