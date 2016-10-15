import state from 'sentinel/state';

var modals = {
    confirm: function(message, callback) {
        state.attr('modal.confirmation.message', message);
        state.attr('modal.confirmation.primaryClick', function() {
            callback();
        });

        $('#modal-confirmation').modal({ show: true });
    }
}

export default modals;