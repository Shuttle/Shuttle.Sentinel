import Component from 'can/component/';
import DefineMap from 'can-define/map/';
import 'can/map/define/';
import view from './register.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import api from '~/api';
import state from '~/state';
import validation from '~/validation';

resources.add('user', { action: 'register', permission: Permissions.Manage.Users });

export const ViewModel = DefineMap.extend({
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
                return validation.get('username', this.username, {
                    username: {
                        presence: true
                    }
                });
            }
        },
    
        passwordConstraint: {
            get: function() {
                return validation.get('password', this.password, {
                    password: {
                        presence: true
                    }
                });
            }
        }
    },

    hasErrors: function() {
        return this.usernameConstraint || this.passwordConstraint;
    },

    register: function() {
        var self = this;

        if (this.hasErrors()) {
            return false;
        }

        this.attr('working', true);

        const user = {
            username: this.username,
            password: this.password
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
    view,
    events: {
        'inserted': function(el) {
            $('#email').focus();
        }
    }
});