import Component from 'can/component/';
import Map from 'can/map/';
import 'can/map/define/';
import './list.less!';
import template from './list.stache!';
import resources from 'sentinel/resources';

resources.add('user', { action: 'list', permission: Permissions.View.User });

export const ViewModel = Map.extend({
  define: {
    message: {
      value: 'This is the sentinel-user-list component'
    }
  }
});

export default Component.extend({
  tag: 'sentinel-user-list',
  viewModel: ViewModel,
  template
});