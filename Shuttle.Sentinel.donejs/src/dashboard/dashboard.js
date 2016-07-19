import Component from 'can/component/';
import Map from 'can/map/';
import 'can/map/define/';
import template from './dashboard.stache!';
import resources from 'sentinel/resources';
import localisation from 'sentinel/localisation';

export const ViewModel = Map.extend({
    define: {
        message: {
            value: 'This is the sentinel-dashboard component'
        }
    }
});

export default Component.extend({
    tag: 'sentinel-dashboard',
    viewModel: ViewModel,
    template
});

localisation.addNamespace('dashboard');
resources.add('dashboard');
