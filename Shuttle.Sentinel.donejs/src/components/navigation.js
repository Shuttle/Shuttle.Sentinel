import can from 'can';
import Map from 'can/map/';
import template from './navigation.stache!';
import state from 'sentinel/application-state';

export default can.Component.extend({
tag: 'sentinel-navigation',
template,
viewModel: state
});