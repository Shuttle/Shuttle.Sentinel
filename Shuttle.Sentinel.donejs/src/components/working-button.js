import can from 'can';
import template from './working-button.stache!';
import security from 'sentinel/security';

export default can.Component.extend({
    tag: 'sentinel-working-button',
    template,
    viewModel: function(attrs, scope) {
        return can.Map.extend({
            define: {
                type: {
                    get: function(type) {
                        return type || 'button';
                    }
                },
                working: {
                    get: function() {
                        return scope._parent._context.attr('working');
                    }
                },
                classType: {
                    get: function(type) {
                        return type || 'btn-primary';
                    }
                },
                classVisibility: {
                    get: function() {
                        const visible = this.attr('visible');
                        return visible != undefined && !visible ? 'hidden' : '';
                    }
                },
                getIconName: {
                    get: function() {
                        return this.attr('working') ? 'glyphicon-hourglass' : this.attr('iconName');
                    }
                },
                getDisabled: {
                    get: function() {
                        return (this.attr('working') || false) || (!this.attr('permission')
                                                                       ? this.attr('disabled') || false
                                                                       : !security.hasPermission(this.attr('permission')));
                    }
                },
                permission: {
                    value: ''
                }
            }
        });
    }
});