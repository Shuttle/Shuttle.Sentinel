import {Component} from 'can';
import ComponentViewModel from 'shuttle-canstrap/infrastructure/component-view-model';
import view from './queue.stache!';

export const ViewModel = ComponentViewModel.extend({
    uri: { type: 'string' }
});

export default Component.extend({
    tag: 'sentinel-queue',
    ViewModel: ViewModel,
    view
});


