import Component from 'can-component';
import DefineMap from 'can-define/map/';
import template from './submit-button.stache!';
import security from '~/security';
import click from '~/components/click';

export const ViewModel = DefineMap.extend({
    define: {
        working: {
            get: function() {
                return this.attr('_parent.working');
            }
        },
        elementClass: {
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
        iconName: {
            get: function(value) {
                return this.attr('working') ? 'glyphicon-hourglass' : value || '';
            }
        },
        disabled: {
            get: function(value) {
                return (this.attr('working') || false) || (!this.attr('permission')
                                                               ? value || false
                                                               : !security.hasPermission(this.attr('permission')));
            }
        },
        permission: {
            value: ''
        }
    },
    _clickHandler: function() {
        click.on(this);
    }
});

export default Component.extend({
    tag: 'sentinel-submit-button',
    template,
    viewModel: function(attrs, scope) {
        let viewModel = new ViewModel(attrs);

        viewModel.attr('_parent', scope._parent._context);

        return viewModel;
    }
});