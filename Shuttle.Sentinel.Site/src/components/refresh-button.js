import Component from 'can-component';
import DefineMap from 'can-define/map/';
import template from './refresh-button.stache!';
import click from '~/components/click';

export default Component.extend({
    tag: 'sentinel-refresh-button',
    template,
    viewModel: Map.extend({
        _clickHandler: function() {
            click.on(this);
        }
    })
});