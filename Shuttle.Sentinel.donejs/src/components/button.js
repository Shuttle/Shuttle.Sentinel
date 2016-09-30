import can from 'can';
import template from './button.stache!';
import security from 'sentinel/security';

export default can.Component.extend({
    tag: 'sentinel-button',
    template,
    viewModel: can.Map.extend({
        define: {
            type: {
                get: function(type) {
                    return type || 'button';
                }
            },
            classType: {
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
                value:''
            }
        }
    })
});