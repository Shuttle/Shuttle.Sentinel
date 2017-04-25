import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import view from './login.stache!';
import resources from '~/resources';
import security from '~/security';
//import validation from '~/validation';

resources.add('user', { action: 'login' });

export const ViewModel = DefineMap.extend(
    'Login',
    {
        username: 'string',
        password: 'string',
        submitIconName: {
            get: function() {
                return this.working ? 'glyphicon-hourglass' : '';
            }
        },
        working: {
            type: 'boolean',
            value: false
        },

        //usernameConstraint: {
        //    get: function() {
        //        return validation.item(this, {
        //            username: {
        //                presence: true
        //            }
        //        });
        //    }

        //hasErrors: function() {
        //    return this.usernameConstraint;
        //},

        login: function() {
            var self = this;

            //if (this.hasErrors()) {
            //    return false;
            //}

            this.working = true;

            security.login({
                username: this.username,
                password: this.password
            })
                .done(function() {
                    window.location.hash = '#!dashboard';
                })
                .always(function() {
                    self.working = false;
                });
        }
    });

export default Component.extend({
    tag: 'sentinel-user-login',
    ViewModel,
    view
});