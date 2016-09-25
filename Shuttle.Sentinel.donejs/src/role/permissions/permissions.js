import Component from 'can/component/';
import Map from 'can/map/';
import 'can/map/define/';
import template from './permissions.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import Model from 'sentinel/model';
import alerts from 'sentinel/alerts';
import localisation from 'sentinel/localisation';

resources.add('role', { action: 'permissions', permission: Permissions.Manage.RolePermissions });

export const ViewModel = Model.extend({
    init: function() {
        this.refresh();
    },

    add: function() {
    },

    refresh: function() {
        this.get('permissions/' + state.attr('route.id'));
    },

    remove: function(id) {
    }
});

export default Component.extend({
    tag: 'sentinel-role-permissions',
    viewModel: ViewModel,
    template
});