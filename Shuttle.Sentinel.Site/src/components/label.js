import can from 'can';
import Map from 'can/map/';
import template from './label.stache!';
import localisation from 'sentinel/localisation';

export const ViewModel = Map.extend({
    define: {
        label: {
            get: function(value) {
                return localisation.value(value);
            }
        }
    }
});

export default can.Component.extend({
    tag: 'sentinel-label',
    template,
    viewModel: ViewModel
});


