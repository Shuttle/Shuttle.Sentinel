import Component from 'can/component/';
import template from './manage.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import api from 'sentinel/api';
import Model from 'sentinel/model';
import alerts from 'sentinel/alerts';
import localisation from 'sentinel/localisation';
import $ from 'jquery';

resources.add('message', { action: 'manage', permission: Permissions.Manage.Users });

export const ViewModel = Model.extend({
    define: {
    },

    init: function() {
        this.refresh();
    },

    refresh: function() {
    },

    remove: function(id) {
    },

});

export default Component.extend({
    tag: 'sentinel-message-manage',
    viewModel: ViewModel,
    template
});