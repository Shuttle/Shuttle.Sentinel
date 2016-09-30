import Component from 'can/component/';
import Map from 'can/map/';
import 'can/map/define/';
import template from './register.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import api from 'sentinel/api';
import state from 'sentinel/state';
import validation from 'sentinel/validation';

resources.add('user', { action: 'register', permission: Permissions.Manage.Users });

export const ViewModel = Map.extend({
    define: {
        username: {
            value: ''
        },
        password: {
            value: ''
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
        },
    
        usernameConstraint: {
            get: function() {
                return validation.get('username', this.attr('username'), {
                    username: {
                        presence: true
                    }
                });
            }
        },
    
        passwordConstraint: {
            get: function() {
                return validation.get('password', this.attr('password'), {
                    password: {
                        presence: true
                    }
                });
            }
        }
    },

    hasErrors: function() {
        return this.attr('usernameConstraint') || this.attr('passwordConstraint');
    },

    register: function() {
        var self = this;

        if (this.hasErrors()) {
            return false;
        }

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

        return true;
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