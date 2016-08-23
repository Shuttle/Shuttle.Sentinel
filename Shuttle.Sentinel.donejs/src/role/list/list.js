import Component from 'can/component/';
import Map from 'can/map/';
import 'can/map/define/';
import './list.less!';
import template from './list.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import Role from 'sentinel/models/role';
import state from 'sentinel/state';

resources.add('role', { action: 'list', permission: Permissions.View.Roles });

export const ViewModel = Map.extend({
    define: {
        roles: {
            get: function() {
                var refresh = this.attr('_refresh');

                return Role.getList();
            }
        }
    },

    add: function() {
        state.goto('role/add');
    },

    refresh: function() {
        this.attr('_refresh', new Date());
    },

    permissions: function(id) {
        state.goto('role/' + id + '/permissions');
    }
});


export default Component.extend({
  tag: 'sentinel-role-list',
  viewModel: ViewModel,
  template
});