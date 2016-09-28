import can from 'can';
import Map from 'can/map/';
import template from './toggle.stache!';

export const ViewModel = Map.extend({
    define: {
        value: {
            value: true
        }
    }
});

export default can.Component.extend({
    tag: 'sentinel-toggle',
    template,
    viewModel: ViewModel
});


