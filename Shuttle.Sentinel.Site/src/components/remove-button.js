import Component from 'can-component';
import template from './remove-button.stache!';
import modals from '~/modals';
import localisation from '~/localisation';
import click from '~/components/click';

export default Component.extend({
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
        _click: function(ev) {
            var self = this;
            var itemName = this.attr('itemName');
            var message = !itemName
                              ? localisation.value('removeItemConfirmation')
                              : localisation.value('removeItemNameConfirmation', { itemName: itemName });

            ev.stopPropagation();

            modals.confirm(message, function() {
                click.on(self);
            });
        }
    })
});