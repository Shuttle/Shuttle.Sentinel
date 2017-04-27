import Component from 'can-component';
import DefineMap from 'can-define/map/';
import view from './submit-button.stache!';
import security from '~/security';
import click from '~/components/click';

export const ViewModel = DefineMap.extend({
    working: {
        get: function() {
            return this.working;
        }
    },
    elementClass: {
        get: function(type) {
            return type || 'btn-primary';
        }
    },
    classVisibility: {
        get: function() {
            const visible = this.visible;
            return visible != undefined && !visible ? 'hidden' : '';
        }
    },
    iconName: {
        get: function(value) {
            return this.working ? 'glyphicon-hourglass' : value || '';
        }
    },
    disabled: {
        get: function(value) {
            return (this.working || false) || (!this.permission
                                                           ? value || false
                                                           : !security.hasPermission(this.permission));
        }
    },
    permission: {
        type: 'string',
        value: ''
    },

    _clickHandler: function() {
        click.on(this);
    }
});

export default Component.extend({
    tag: 'sentinel-submit-button',
    view,
    ViewModel
});