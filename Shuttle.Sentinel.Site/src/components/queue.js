import Component from 'can-component';
import InputViewModel from './input-view-model';
import view from './queue.stache!';

export const ViewModel = InputViewModel.extend({
    uri: { type: 'string' }
});

export default Component.extend({
    tag: 'cs-queue',
    ViewModel: ViewModel,
    view
});


