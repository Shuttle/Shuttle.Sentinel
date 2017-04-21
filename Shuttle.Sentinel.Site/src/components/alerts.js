import Component from 'can-component';
import view from './alerts.stache!';
import alerts from '~/alerts';

export default Component.extend({
    tag: 'sentinel-alerts',
    view,
    viewModel: alerts
});