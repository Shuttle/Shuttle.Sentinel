import can from 'can';
import Map from 'can/map/';
import template from './fetching.stache!';
import localisation from 'sentinel/localisation';

export const ViewModel = Map.extend({
    define: {
        title: {
            get: function(value) {
                return localisation.value('fetching', { name: localisation.value(value)});
            }
        }
    }
});

export default can.Component.extend({
    tag: 'sentinel-fetching',
    template,
    viewModel: ViewModel
});