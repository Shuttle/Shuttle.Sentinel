import Component from 'can/component/';
import Map from 'can/map/';
import can from 'can';
import 'can/map/define/';
import './list.less!';
import template from './list.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import Role from 'sentinel/models/role';
import state from 'sentinel/state';
import List from 'sentinel/list-model';

resources.add('role', { action: 'list', permission: Permissions.View.Roles });

export const ViewModel = List.extend({
    init: function() {
        this.refresh();
    },

    add: function() {
        state.goto('role/add');
    },

    refresh: function() {
        this.fetch('roles');
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