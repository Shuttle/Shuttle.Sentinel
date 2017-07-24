import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './refresh-button.stache!';
import click from '~/components/click';

export const ViewModel = DefineMap.extend({
    context: {
        value: null
    },
    _click: function(ev) {
        ev.stopPropagation();

        return click.on(this);
    }
});

export default Component.extend({
    tag: 'sentinel-refresh-button',
    ViewModel,
    view
});