import Component from 'can/component/';
import Map from 'can/map/';
import 'can/map/define/';
import './roles.less!';
import template from './roles.stache!';
import Permissions from 'sentinel/permissions';
import resources from 'sentinel/resources';

resources.add('user', { action: 'roles', permission: Permissions.Manage.UserRoles });

export const ViewModel = Map.extend({
  define: {
    message: {
      value: 'This is the sentinel-user-roles component'
    }
  }
});

export default Component.extend({
  tag: 'sentinel-user-roles',
  viewModel: ViewModel,
  template
});