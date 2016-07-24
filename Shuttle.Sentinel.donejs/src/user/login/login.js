import Component from 'can/component/';
import Map from 'can/map/';
import 'can/map/define/';
import './login.less!';
import template from './login.stache!';
import resources from 'sentinel/resources';
import api from 'sentinel/api';
import security from 'sentinel/security';

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
        }
    },

    login: function() {
        var self = this;
        this.attr('working', true);

        security.login(this.attr('username'), this.attr('password'))
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