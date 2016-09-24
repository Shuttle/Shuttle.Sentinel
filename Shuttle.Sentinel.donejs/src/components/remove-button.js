import can from 'can';
import template from './remove-button.stache!';
import modals from 'sentinel/modals';
import localisation from 'sentinel/localisation';

export default can.Component.extend({
    tag: 'sentinel-remove-button',
    template,
    viewModel: can.Map.extend({
        _click: function() {
            var itemName = this.attr('itemName');
            var message = !itemName
                              ? localisation.value('removeItemConfirmation')
                              : localisation.value('removeItemNameConfirmation', { itemName: itemName });

            modals.confirm(this, message, arguments);
        }
    })
});