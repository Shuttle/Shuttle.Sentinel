import can from 'can';
import template from './alerts.stache!';
import state from 'sentinel/application-state';

export default can.Component.extend({
    tag: 'sentinel-alerts',
    template,
    viewModel: state
});