import can from 'can';
import Map from 'can/map/';
import template from './navigation.stache!';
import state from 'sentinel/application-state';
import security from 'sentinel/security';

export const ViewModel = Map.extend({
    define: {
        security: {
            value: security
        },
        state: {
            value: state
        }
    }
});

export default can.Component.extend({
    tag: 'sentinel-navigation',
    template,
    viewModel: ViewModel
});