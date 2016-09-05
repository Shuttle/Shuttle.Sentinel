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
import 'validate.js';

resources.add('role', { action: 'add', permission: Permissions.Add.Role});

var constraints = {
    name: {
        presence: true
    }
};

export const ViewModel = Map.extend({
    define: {
        name: {
            value: ''
        }
    },

    add: function() {
        var result = validate(this, constraints);

        var role = new Role({
            name: this.attr('name')
        });

        role.save();
    }
});

export default Component.extend({
    tag: 'sentinel-role-add',
    viewModel: ViewModel,
    template
});