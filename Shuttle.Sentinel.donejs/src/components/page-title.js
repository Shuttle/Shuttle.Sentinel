import can from 'can';
import Map from 'can/map/';
import template from './page-title.stache!';
import localisation from 'sentinel/localisation';

export const ViewModel = Map.extend({
    define: {
        title: {
            get: function(value) {
                return localisation.value(value);
            }
        }
    }
});

export default can.Component.extend({
    tag: 'sentinel-page-title',
    template,
    viewModel: ViewModel
});


