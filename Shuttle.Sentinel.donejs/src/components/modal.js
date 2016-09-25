import can from 'can';
import Map from 'can/map/';
import template from './modal.stache!';
import localisation from 'sentinel/localisation';

export const ViewModel = Map.extend({
    define: {
        modalType: {
            get: function(value) {
                return value || 'fade';
            }
        },

        dismissText: {
          get: function(value) {
              return value || localisation.value(value);
          }
        },

        textType: {
            get: function(value) {
                return value || 'primary';
            }
        },

        message: {
            value: ''
        },

        hasMessage: {
            get: function() {
                return !!this.attr('message');
            }
        }
    },

    _primaryClick: function() {
        var modalElement = $('#' + this.attr('modalId'));

        if (!this.__bindEvents['primaryClick']) {
            alert('Assign a primary click handler by adding \'something\' to your modal component definition.');
            return;
        }

        if (modalElement) {
            modalElement.modal('hide');
        }

        can.trigger(this, 'primaryClick', arguments);
    }
});

export default can.Component.extend({
    tag: 'sentinel-modal',
    template,
    viewModel: ViewModel
});