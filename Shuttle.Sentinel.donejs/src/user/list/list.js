import Component from 'can/component/';
import Map from 'can/map/';
import 'can/map/define/';
import './list.less!';
import template from './list.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import User from 'sentinel/models/user';
import state from 'sentinel/state';

resources.add('user', { action: 'list', permission: Permissions.View.Users });

export const ViewModel = Map.extend({
    define: {
        users: {
            value: function() {
                return User.getList();
            }
        }
    },

    add: function() {
        state.goto('user/register');
    },

    refresh: function() {
        
    },

    roles: function(id) {
        state.goto('user/' + id + '/roles');
    }
});

export default Component.extend({
    tag: 'sentinel-user-list',
    viewModel: ViewModel,
    template
});