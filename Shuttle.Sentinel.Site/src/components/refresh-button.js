import can from 'can';
import Map from 'can/map/';
import template from './refresh-button.stache!';
import click from 'sentinel/components/click';

export default can.Component.extend({
    tag: 'sentinel-refresh-button',
    template,
    viewModel: Map.extend({
        _clickHandler: function() {
            click.on(this);
        }
    })
});