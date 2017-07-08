import Component from 'can-component';
import InputViewModel from './input-view-model';
import view from './queue.stache!';

export default Component.extend({
    tag: 'sentinel-queue',
    ViewModel: InputViewModel,
    view
});


