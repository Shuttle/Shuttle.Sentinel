import Component from 'can/component/';
import Map from 'can/map/';
import 'can/map/define/';
import './list.less!';
import template from './list.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import User from 'sentinel/models/user';

resources.add('user', { action: 'list', permission: Permissions.View.Users });

export const ViewModel = Map.extend({
  define: {
    users: {
      value: function() {
          return User.getList();
      }
    }
  }
});

export default Component.extend({
  tag: 'sentinel-user-list',
  viewModel: ViewModel,
  template
});