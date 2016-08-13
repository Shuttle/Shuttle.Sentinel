import Component from 'can/component/';
import Map from 'can/map/';
import 'can/map/define/';
import './register.less!';
import template from './register.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import api from 'sentinel/api';
import state from 'sentinel/state';

resources.add('user', { action: 'register', permission: Permissions.Register.User });

export const ViewModel = Map.extend({
    define: {
        username: {
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
        },
        title: {
            get: function() {
                return state.isUserRequired ? 'user:register.user-required' : 'user:register.title';
            }
        },
        showClose: {
            get: function() {
                return !state.isUserRequired;
            }
        }
    },

    register: function() {
        var self = this;
        this.attr('working', true);
        const user = {
            username: this.attr('username'),
            password: this.attr('password')
        };
        api.post('users', { data: user })
            .done(function() {
                if (state.isUserRequired) {
                    state.isUserRequired = false;

                    state.goto('dashboard');
                } else {
                    state.goto('user/list');
                }
            })
            .always(function() {
                self.attr('working', false);
            });
    },

    close: function() {
        state.goto('user/list');
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