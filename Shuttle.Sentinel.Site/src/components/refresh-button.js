import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './refresh-button.stache!';

export default Component.extend({
    tag: 'sentinel-refresh-button',
    view,
    ViewModel: DefineMap.extend({
    })
});