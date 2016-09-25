import can from 'can';
import state from 'sentinel/state';

var modals = {
    confirm: function(target, message, args) {
        state.attr('modal.confirmation.message', message);
        state.attr('modal.confirmation.primaryClick', function() {
            can.trigger(target, 'click', args);
        });

        $('#modal-confirmation').modal({ show: true });
    }
}

export default modals;