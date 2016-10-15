import can from 'can';
import template from './remove-button.stache!';
import modals from 'sentinel/modals';
import localisation from 'sentinel/localisation';
import click from 'sentinel/components/click';

export default can.Component.extend({
    tag: 'sentinel-remove-button',
    template,
    viewModel: can.Map.extend({
        define: {
            elementClass: {
                get: function(value) {
                    return value || '';
                }
            }
        },
        _click: function() {
            var self = this;
            var itemName = this.attr('itemName');
            var message = !itemName
                              ? localisation.value('removeItemConfirmation')
                              : localisation.value('removeItemNameConfirmation', { itemName: itemName });

            modals.confirm(message, function() {
                click.on(self);
            });
        }
    })
});