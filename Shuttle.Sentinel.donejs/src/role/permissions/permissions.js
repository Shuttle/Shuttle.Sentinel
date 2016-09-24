import Component from 'can/component/';
import Map from 'can/map/';
import 'can/map/define/';
import template from './permissions.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';

resources.add('role', { action: 'permissions', permission: Permissions.Manage.RolePermissions });

export const ViewModel = Map.extend({
    define: {
        role: {
            value: function() {
            }
        }
    }
});

export default Component.extend({
    tag: 'sentinel-role-permissions',
    viewModel: ViewModel,
    template
});