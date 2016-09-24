import can from 'can';

var modals = {
    confirm: function(target, message, args) {
        let viewModel = $('#modal-confirmation').viewModel();

        viewModel.attr('message', message);

        viewModel.primaryClickTarget = function() {
            can.trigger(target, 'click', args);
        }

        $('#modal-confirmation').modal({ show: true });
    }
}

export default modals;