import Component from 'can/component/';
import can from 'can/';
import './add.less!';
import template from './add.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import Role from 'sentinel/models/role';
import api from 'sentinel/api';
import state from 'sentinel/state';

import validation from 'sentinel/validation';

resources.add('role', { action: 'add', permission: Permissions.Add.Role});

export const ViewModel = can.Map.extend({
    define: {
        name: {
            value: ''
        },

        nameConstraint: {
            get: function() {
                return validation.get('name', this.attr('name'), {
                    name: {
                        presence: true
                    }
                });
            }
        }
    },

    hasErrors: function() {
        return this.attr('nameConstraint');
    },

    add: function() {
        if (this.hasErrors()) {
            return false;
        }

        var role = new Role({
            name: this.attr('name')
        });

        role.save();

        return false;
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