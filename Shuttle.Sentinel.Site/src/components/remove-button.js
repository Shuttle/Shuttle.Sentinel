import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './remove-button.stache!';
import modals from '~/modals';
import localisation from '~/localisation';
import click from '~/components/click';

export const ViewModel = DefineMap.extend({
    elementClass: {
        get: function(value) {
            return value || '';
        }
    },

    _click: function(ev) {
        var self = this;
        const itemName = this.itemName;
        const message = !itemName
            ? localisation.value('removeItemConfirmation')
            : localisation.value('removeItemNameConfirmation', { itemName: itemName });

        ev.stopPropagation();

        modals.confirm(message, function() {
            click.on(self);
        });
    }
})

export default Component.extend({
    tag: 'sentinel-remove-button',
    view,
    ViewModel
});