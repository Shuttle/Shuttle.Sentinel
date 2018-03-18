import Component from 'can-component';
import ComponentViewModel from 'shuttle-canstrap/infrastructure/component-view-model';
import view from './queue.stache!';

export const ViewModel = ComponentViewModel.extend({
    uri: { type: 'string' }
});

export default Component.extend({
    tag: 'cs-queue',
    ViewModel: ViewModel,
    view
});


