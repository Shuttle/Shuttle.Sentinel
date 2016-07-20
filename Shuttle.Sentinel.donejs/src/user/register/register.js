import Component from 'can/component/';
import Map from 'can/map/';
import 'can/map/define/';
import './register.less!';
import template from './register.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';

resources.add('user', { action: 'register', permission: Permissions.Register.User });

export const ViewModel = Map.extend({
    define: {
        email: {
            value: ''
        },
        password: {
            value: ''
        },
        submitIconName: {
            get: function() {
                return this.attr('working') ? 'glyphicon-hourglass' : '';
            }
        },
        working: {
            value: false
        }
    },

    submit: function() {
        this.attr('working', true);
    }
});

export default Component.extend({
    tag: 'sentinel-user-register',
    viewModel: ViewModel,
    template,
    events: {
        'inserted': function(el) {
            $('#email').focus();
        }
    }
});

