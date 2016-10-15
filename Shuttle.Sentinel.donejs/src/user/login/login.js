import Component from 'can/component/';
import Map from 'can/map/';
import 'can/map/define/';
import template from './login.stache!';
import resources from 'sentinel/resources';
import security from 'sentinel/security';
import validation from 'sentinel/validation';

resources.add('user', { action: 'login' });

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

        usernameConstraint: {
            get: function() {
                return validation.item(this, {
                    username: {
                        presence: true
                    }
                });
            }
        }
    },

    hasErrors: function() {
        return this.attr('usernameConstraint');
    },

    login: function() {
        var self = this;

        if (this.hasErrors()) {
            return false;
        }

        this.attr('working', true);

        security.login({
            username: this.attr('username'),
            password: this.attr('password')
        })
            .done(function() {
                window.location.hash = '#!dashboard';
            })
            .always(function() {
                self.attr('working', false);
            });
    }
});

export default Component.extend({
    tag: 'sentinel-user-login',
    viewModel: ViewModel,
    template
});