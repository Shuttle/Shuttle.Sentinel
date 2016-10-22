import Component from 'can/component/';
import template from './properties.stache!';
import ViewModel from './properties-model';

export default Component.extend({
    tag: 'sentinel-datastore-properties',
    viewModel: ViewModel,
    template
});