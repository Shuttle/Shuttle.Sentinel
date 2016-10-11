import can from 'can';
import Map from 'can/map/';
import template from './button.stache!';
import security from 'sentinel/security';

export const ViewModel = Map.extend({
    define: {
        buttonType: {
            get: function() {
                return (this.attr('actions') && this.attr('actions').length > 0)
                           ? 'dropdown'
                           : 'button';
            }
        },
        type: {
            get: function(type) {
                return type || 'button';
            }
        },
        elementClass: {
            get: function(type) {
                return type || 'btn-primary';
            }
        },
        classVisibility: {
            get: function() {
                var visible = this.attr('visible');

                return visible != undefined && !visible ? 'hidden' : '';
            }
        },
        iconName: {
            value: ''
        },
        disabled: {
            get: function(value) {
                var disabled = value || false;

                if (this.attr('permission') && !disabled) {
                    disabled = !security.hasPermission(this.attr('permission'));
                }

                return disabled;
            }
        },
        permission: {
            value: ''
        }
    }
});

export default can.Component.extend({
    tag: 'sentinel-button',
    template,
    viewModel: ViewModel
});