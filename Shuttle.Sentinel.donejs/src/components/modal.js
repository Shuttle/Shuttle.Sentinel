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

        hasMessage: {
            get: function() {
                return !!this.attr('message');
            }
        }
    },

    primaryClick: function() {
        this.primaryClickTarget.call();
    }
});

export default can.Component.extend({
    tag: 'sentinel-modal',
    template,
    viewModel: ViewModel
});