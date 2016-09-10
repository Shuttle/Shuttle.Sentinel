import Component from 'can/component/';
import Map from 'can/map/';
import 'can/map/define/';
import './add.less!';
import template from './add.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import Role from 'sentinel/models/role';
import api from 'sentinel/api';
import state from 'sentinel/state';
import 'sentinel/can-validate';
import 'sentinel/validations';

resources.add('role', { action: 'add', permission: Permissions.Add.Role});

export const ViewModel = Map.extend({
    define: {
        name: {
            value: '',
            validate: {
                required: true
            }
        }
    },

    init: function() {
        this.validate();
    },

    add: function() {
        if (!this.validate()) {
            return;
        }

        var role = new Role({
            name: this.attr('name')
        });

        role.save();
    },

    close: function() {
        state.goto('role/list');
    }
});

export default Component.extend({
    tag: 'sentinel-role-add',
    viewModel: ViewModel,
    template
});