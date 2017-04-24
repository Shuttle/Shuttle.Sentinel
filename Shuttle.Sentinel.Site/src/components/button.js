import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './button.stache!';
import security from '~/security';
import click from '~/components/click';

export const ViewModel = DefineMap.extend({
    define: {
        context: {
            value: null
        },
        buttonType: {
            get: function() {
                return (this.actions && this.actions.length > 0)
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
                var visible = this.visible;

                return visible != undefined && !visible ? 'hidden' : '';
            }
        },
        iconName: {
            get: function(value) {
                return value || '';
            }
        },
        disabled: {
            get: function(value) {
                var disabled = value || false;

                if (this.permission && !disabled) {
                    disabled = !security.hasPermission(this.permission);
                }

                return disabled;
            }
        },
        permission: {
            value: ''
        }
    },

    _clickHandler: function(ev) {
        click.on(this, ev);
    }
});

export default Component.extend({
    tag: 'sentinel-button',
    view,
    viewModel: ViewModel
});